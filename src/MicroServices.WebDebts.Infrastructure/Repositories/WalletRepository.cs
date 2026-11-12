using MicroServices.WebDebts.Domain.Interfaces.Repository;
using MicroServices.WebDebts.Domain.Models;
using MicroServices.WebDebts.Domain.Models.Commom;
using MicroServices.WebDebts.Domain.Models.Enum;
using MicroServices.WebDebts.Infrastructure.Database.Postgres;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MicroServices.WebDebts.Infrastructure.Repositories
{
    public class WalletRepository : BaseRepository<Wallet>, IWalletRepository
    {
        private readonly DataContext _context;
        private DbSet<Wallet> _dbSet;
        private DbSet<WalletInstallments> _dbSetInstallment;

        public WalletRepository(DataContext context) : base(context)
        {
            _dbSet = context.Set<Wallet>();
            _dbSetInstallment = context.Set<WalletInstallments>();
            _context = context;
        }

        public async Task<List<Wallet>> GetWallets(WalletStatus? walletStatus, int month, int year, Guid userId)
        {

            if (!walletStatus.HasValue)
            {
                var resultQuery = await _dbSet.Include(x => x.ResponsibleParty).Where(x => x.WalletStatus == walletStatus && x.User.Id == userId).ToListAsync();
                return resultQuery;
            }
            else
            {
                var resultQuery = await _dbSet.Include(x => x.ResponsibleParty).Include(x => x.WalletInstallments.Where(x => x.Date.Month == month && x.Date.Year == year))
                    .Where(x => x.WalletStatus != WalletStatus.Disable && x.User.Id == userId).ToListAsync();
                return resultQuery;
            }
        }

        public async Task<List<Wallet>> GetWalletByMonth(int? month, int? year, Guid userId)
        {
            var resultQuery = await _dbSet.Include(x => x.WalletInstallments).Where(x => x.WalletStatus != WalletStatus.Disable && x.User.Id == userId && x.WalletStatus != WalletStatus.Pending).ToListAsync();

            return resultQuery;
        }

        public override async Task<Wallet> FindByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                return null;

            var wallet = await _dbSet.Where(x => x.Id == id).Include(x => x.User).FirstOrDefaultAsync();

            if (wallet == null)
                return null;

            return wallet;
        }

        public async Task<List<WalletInstallments>> GetWalletInstallmentByMonth(int? month, int? year, Guid userId)
        {
            var startDate = new DateTime(year.Value, month.Value, 1, 0, 0, 0);

            var futureDate = startDate.AddMonths(4);

            var finishDate = new DateTime(futureDate.Year, futureDate.Month, DateTime.DaysInMonth(futureDate.Year, futureDate.Month));


            var walletInstallments = _dbSet.Where(x => x.WalletStatus != WalletStatus.Disable).Include(x => x.WalletInstallments).SelectMany(x => x.WalletInstallments).AsQueryable();
            walletInstallments = walletInstallments.Where(x => x.User.Id == userId);
            walletInstallments = walletInstallments.Where(x => x.Date >= startDate.Date);
            walletInstallments = walletInstallments.Where(x => x.Date <= finishDate.Date);


            return await walletInstallments.ToListAsync();
        }

        public async Task<Wallet> GetWalletByIdAsync(Guid id)
        {
            return await _dbSet.Where(x => x.Id == id).Include(x => x.WalletInstallments).FirstOrDefaultAsync();
        }

        public async Task<PaginatedList<WalletInstallments>> GetWalletInstallmentsAsync(Guid walletId, Guid userId, int pageNumber, int pageSize)
        {
            var query = _dbSetInstallment
                .Where(installment => installment.Wallet.Id == walletId && installment.Wallet.User.Id == userId)
                .OrderBy(installment => installment.Date)
                .ThenBy(installment => installment.InstallmentNumber)
                .ThenBy(installment => installment.Id);

            var totalItems = await query.CountAsync();
            var totalPages = Convert.ToInt32(totalItems / pageSize + (totalItems % pageSize > 0 ? 1 : 0));
            var installments = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedList<WalletInstallments>
            {
                CurrentPage = pageNumber,
                TotalPages = totalPages,
                TotalItems = totalItems,
                Items = installments
            };
        }

        public async Task<WalletInstallments> GetInstallmentById(Guid id, Guid userId)
        {
            return await _dbSetInstallment
                .FirstOrDefaultAsync(installment => installment.Id == id && installment.Wallet.User.Id == userId);
        }

        public async Task<List<Wallet>> GetWalletResposibleParty(Guid? responsiblePartyId, int month, int year, Guid userId)
        {
            var walletResponsibleParty = _dbSet.Include(x => x.ResponsibleParty).Include(x => x.WalletInstallments).Where(x => x.User.Id == userId).Where(x => x.WalletStatus != WalletStatus.Disable);
            if (responsiblePartyId.HasValue)
            {
                walletResponsibleParty = walletResponsibleParty.Where(x => x.ResponsibleParty.Id == responsiblePartyId.Value);
            }

            await walletResponsibleParty.ToListAsync();

            var result = new List<Wallet>();

            foreach (var wallet in walletResponsibleParty)
            {
                var instalments = wallet.WalletInstallments.Where(x => x.Date.Month == month && x.Date.Year == year).ToList();
                if (instalments.Any())
                {
                    wallet.WalletInstallments = instalments;
                    result.Add(wallet);
                }
            }

            return result;
        }

        public async Task<List<WalletInstallments>> GetWalletInstallmentBySpecificMonth(int? month, int? year, Guid userId)
        {
            var walletInstallments = _dbSet.Where(x => x.WalletStatus != WalletStatus.Disable).Include(x => x.WalletInstallments).SelectMany(x => x.WalletInstallments).AsQueryable();
            walletInstallments = walletInstallments.Where(x => x.User.Id == userId);
            walletInstallments = walletInstallments.Where(x => x.Date.Month == month && x.Date.Year == year);
            return await walletInstallments.ToListAsync();
        }
    }
}

