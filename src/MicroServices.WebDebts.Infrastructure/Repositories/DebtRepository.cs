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

        public async Task<List<Debt>> FindDebtAsync(string name, decimal? value, DateTime? startDate, DateTime? finishDate, DebtInstallmentType? debtInstallmentType, DebtType? debtType)
        {
            var query = _dbSet.Include(x => x.Installments);

            var queryFilter = await DebtsFilters(query, name, value, startDate, finishDate, debtInstallmentType, debtType).ToListAsync();

            var result = new List<Debt>();

            if (startDate.HasValue || finishDate.HasValue)
            {
                foreach (var debt in queryFilter)
                {
                    var instalments = debt.Installments.Where(x => x.Date >= startDate && x.Date <= finishDate).ToList();
                    debt.Installments = instalments;
                    result.Add(debt);
                }
            }
            else
                return queryFilter; 

            return result;
        }

        public async Task<Debt> GetAllByIdAsync(Guid Id)
        {
            var resultQuery = _dbSet.Include(p => p.Installments)
                                    .Where(x => x.Id == Id).FirstOrDefault();

            resultQuery.Installments = resultQuery.Installments.OrderBy(x => x.Date).ToList();

            return resultQuery;
        }

        private static IQueryable<Debt> DebtsFilters(IQueryable<Debt> resultQuery,
                                                                      string name, 
                                                                      decimal? value, 
                                                                      DateTime? startDate, 
                                                                      DateTime? finishDate, 
                                                                      DebtInstallmentType? debtInstallmentType, 
                                                                      DebtType? debtType)
        {
            if (!String.IsNullOrEmpty(name))
            {
                resultQuery = resultQuery.Where(p => p.Name.ToLower().Contains(name.ToLower()));
            }
            if (value != null)
            {
                resultQuery = resultQuery.Where(p => p.Value == value);
            }
            if (debtInstallmentType != null)
            {
                resultQuery = resultQuery.Where(p => p.DebtInstallmentType == debtInstallmentType);
            }
            if (debtType != null)
            {
                resultQuery = resultQuery.Where(p => p.DebtType == debtType);
            }

            return resultQuery;
        }

        public async Task<List<Installments>> FilterInstallmentsAsync(Guid? debtId, int? month, int? year, DebtInstallmentType? debtInstallmentType, Status? status)
        {
            var installments = _context.Debt.Include(x => x.Installments).SelectMany(x => x.Installments).AsQueryable();

            if (debtId.HasValue)
                installments = _context.Debt.Include(x => x.Installments).Where(x => x.Id == debtId.Value).SelectMany(x => x.Installments).AsQueryable();
            
            if (debtInstallmentType.HasValue)
                installments = _context.Debt.Include(x => x.Installments).Where(x => x.DebtInstallmentType == debtInstallmentType.Value).SelectMany(x => x.Installments).AsQueryable();
            
            var resultFilter = InstallmentsFilters(installments, month, year, status);

            return await resultFilter.ToListAsync();
        }

        private static IQueryable<Installments> InstallmentsFilters(IQueryable<Installments> resultQuery, int? month, int? year, Status? status)
        {
            if (month.HasValue)
            {
                resultQuery = resultQuery.Where(x => x.Date.Month == month.Value);
            }
            if (year.HasValue)
            {
                resultQuery = resultQuery.Where(x => x.Date.Year == year.Value);
            }
            if (status.HasValue)
            {
                resultQuery = resultQuery.Where(x => x.Status == status.Value);
            }

            return resultQuery;
        }

        public async Task UpdateInstallmentAsync(Guid id, Status status)
        {
            var installment = await _context.Debt.SelectMany(x => x.Installments.Where(x => x.Id == id)).FirstOrDefaultAsync();

            installment.Status = status;

            if (status == Status.Paid)
                installment.PaymentDate = DateTime.Now;
            else
                installment.PaymentDate = null;
        }

        public async Task<List<Installments>> GetSumPerMonthAsync(int? month, int? year)
        {
            var startDate = new DateTime(year.Value, month.Value, 1, 0, 0, 0);
            var finishDate = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 31);


            var installments = _context.Debt.Include(x => x.Installments).SelectMany(x => x.Installments).AsQueryable();
            installments = installments.Where(x => x.Date >= startDate.Date);
            installments = installments.Where(x => x.Date <= finishDate.Date);

            return await installments.ToListAsync();
        }
    }
}
