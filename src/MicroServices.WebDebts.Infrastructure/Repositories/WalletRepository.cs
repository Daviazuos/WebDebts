﻿using MicroServices.WebDebts.Domain.Interfaces.Repository;
using MicroServices.WebDebts.Domain.Models;
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

        public WalletRepository(DataContext context) : base(context)
        {
            _dbSet = context.Set<Wallet>();
            _context = context;
        }

        public async Task<List<Wallet>> GetWallets(WalletStatus? walletStatus, int month, int year, Guid userId)
        {

            if (!walletStatus.HasValue)
            {
                var resultQuery = await _dbSet.Where(x => x.WalletStatus == walletStatus && x.User.Id == userId).ToListAsync();
                return resultQuery;
            }
            else
            {
                var resultQuery = await _dbSet.Include(x => x.WalletInstallments.Where(x => x.Date.Month == month && x.Date.Year == year))
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
    }

}

