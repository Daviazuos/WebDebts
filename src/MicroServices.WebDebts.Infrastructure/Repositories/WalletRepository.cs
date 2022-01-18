using MicroServices.WebDebts.Domain.Interfaces.Repository;
using MicroServices.WebDebts.Domain.Models;
using MicroServices.WebDebts.Domain.Models.Enum;
using MicroServices.WebDebts.Infrastructure.Database.Postgres;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroServices.WebDebts.Infrastructure.Repositories
{
    public class WalletRepository : BaseRepository<Wallet>, IWalletRepository
    {
        private readonly DataContext _context;
        private DbSet<Wallet> _dbSet;

        public WalletRepository(DataContext context) : base(context)
        {
            _dbSet = context.Set<Wallet>();
            _context = context;
        }

        public async Task<List<Wallet>> GetWallets(WalletStatus walletStatus)
        {
            var actualDate = DateTime.Now;

            var resultQuery = await _dbSet.Where(x => x.FinishAt == null && x.WalletStatus == walletStatus)
                                          .Include(x => x.WalletMonthControllers.Where(x => x.Month == actualDate.Month && x.Year == actualDate.Year)).ToListAsync();


            return resultQuery;
        }

        public async Task<List<Wallet>> GetWalletByMonth(int? month, int? year)
        {
            var startDate = new DateTime(year.Value, month.Value, 1, 0, 0, 0);
            var finishDate = DateTime.UtcNow.Date;

            var resultQuery = _dbSet.Where(x => x.WalletStatus == WalletStatus.Enable);

            return await resultQuery.ToListAsync();
        }

        public async Task<Guid> SubtractWalletMonthControllers(Guid walletId, int month, int year, decimal value)
        {
            var wallet = await _dbSet.Where(x => x.Id == walletId).FirstOrDefaultAsync();

            if (wallet.WalletMonthControllers != null)
            {
                var walletControllers = wallet.WalletMonthControllers.Where(x => x.Month == month).FirstOrDefault();

                var walletMonthControlerModel = new WalletMonthController
                {
                    CreatedAt = walletControllers.CreatedAt,
                    Month = month,
                    Year = year,    
                    UpdatedValue = walletControllers.UpdatedValue - value,
                    Id = walletControllers.Id,
                    Wallet = wallet
                };

                walletControllers = walletMonthControlerModel;
                _context.WalletMonthControllers.Update(walletControllers);
                return walletControllers.Id;
            }
            else
            {
                var walletMonthControler = new WalletMonthController
                {
                    CreatedAt = DateTime.Now,
                    Month = month,
                    Year = year,
                    UpdatedValue = wallet.Value - value,
                    Id = Guid.NewGuid(),
                    Wallet = wallet
                };

                await _context.WalletMonthControllers.AddAsync(walletMonthControler);
                return walletMonthControler.Id;
            }

        }

        public async Task<Guid> AddWalletMonthControllers(Guid walletMonthId, decimal value)
        {

            var walletControllers = _context.WalletMonthControllers.Where(x => x.Id == walletMonthId).FirstOrDefault();

            walletControllers.UpdatedValue = walletControllers.UpdatedValue + value;

            _context.WalletMonthControllers.Update(walletControllers);
            return walletControllers.Id;

        }
    }

}

