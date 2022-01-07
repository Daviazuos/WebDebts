using MicroServices.WebDebts.Domain.Interfaces.Repository;
using MicroServices.WebDebts.Domain.Models;
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

        public async Task<List<Wallet>> GetEnableWallet()
        {
            var resultQuery = await _dbSet.Where(x => x.FinishAt == null).ToListAsync() ;

            return resultQuery;
        }

        public async Task<List<Wallet>> GetWalletByMonth(int? month, int? year)
        {
            var startDate = new DateTime(year.Value, month.Value, 1, 0, 0, 0);
            var finishDate = DateTime.UtcNow.Date;

            var resultQuery = _dbSet.Where(x => x.WalletStatus == Domain.Models.Enum.WalletStatus.Enable);

            return await resultQuery.ToListAsync();
        }
    }

}

