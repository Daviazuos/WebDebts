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
    public class DebtRepository : BaseRepository<Debt>, IDebtRepository
    {
        private readonly DataContext _context;
        private DbSet<Debt> _dbSet;
        private DbSet<Installments> _dbSetInstallment;

        public DebtRepository(DataContext context) : base(context)
        {
            _dbSet = context.Set<Debt>();
            _dbSetInstallment = context.Set<Installments>();
            _context = context;
        }

        public async Task DeleteDebt(Guid Id)
        {
            var debtToRemove = _dbSet.Where(x => x.Id == Id)
                                     .Include(x => x.Installments)
                                     .ToList();

            _dbSet.RemoveRange(debtToRemove);
            _dbSetInstallment.RemoveRange(debtToRemove.SelectMany(x => x.Installments));
        }

        public async Task<List<Debt>> FindDebtAsync(string name, decimal? value, DateTime? date, DebtInstallmentType? debtInstallmentType, DebtType? debtType)
        {
            var resultQuery = _dbSet.Include(p => p.Installments)
                                    .Select(x => x);

            if (!String.IsNullOrEmpty(name))
            {
                resultQuery = resultQuery.Where(p => p.Name.ToLower().Contains(name.ToLower()));
            }
            if (value != null)
            {
                resultQuery = resultQuery.Where(p => p.Value == value);
            }
            if (date != null)
            {
                resultQuery = resultQuery.Where(p => p.Date == date);
            }
            if (debtInstallmentType != null)
            {
                resultQuery = resultQuery.Where(p => p.DebtInstallmentType == debtInstallmentType);
            }
            if (debtType != null)
            {
                resultQuery = resultQuery.Where(p => p.DebtType == debtType);
            }

            return await resultQuery.ToListAsync();
        }

        public async Task<Debt> GetAllByIdAsync(Guid Id)
        {
            var resultQuery = _dbSet.Include(p => p.Installments)
                                    .Where(x => x.Id == Id).FirstOrDefault();

            resultQuery.Installments = resultQuery.Installments.OrderBy(x => x.Date).ToList();

            return resultQuery;
        }
    }
}
