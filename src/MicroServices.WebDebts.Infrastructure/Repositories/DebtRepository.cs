using MicroServices.WebDebts.Domain.Interfaces.Repository;
using MicroServices.WebDebts.Domain.Models;
using MicroServices.WebDebts.Domain.Models.Commom;
using MicroServices.WebDebts.Domain.Models.Enum;
using MicroServices.WebDebts.Infrastructure.Database.Postgres;
using MicroServices.WebDebts.Infrastructure.Migrations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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

        public async Task DeleteInstallment(Guid Id)
        {
            var installmentToRemove = _dbSetInstallment.Where(x => x.Id == Id).FirstOrDefault();

            _dbSetInstallment.Remove(installmentToRemove);
        }

        public async Task<PaginatedList<Debt>> FindDebtAsync(int pageNumber, int pageSize, string name, decimal? value, DateTime? startDate, DateTime? finishDate, DebtInstallmentType? debtInstallmentType, DebtType? debtType, string category, Guid userId, bool? isGoal)
        {
            var query = _dbSet.Include(x => x.Installments).Include(x => x.DebtCategory).Include(x => x.Card);

            var resultQuery = DebtsFilters(query, name, value, startDate, finishDate, debtInstallmentType, debtType, category, isGoal);

            resultQuery = resultQuery.Where(x => x.User.Id == userId);

            var skipNumber = pageNumber > 0 ? ((pageNumber - 1) * pageSize) : 0;
            var totalItems = resultQuery.Count();
            var totalPages = Convert.ToInt32(totalItems / pageSize + (totalItems % pageSize > 0 ? 1 : 0));

            var resultFilter = await resultQuery.OrderByDescending(x => x.BuyDate)
                                                .Skip(skipNumber)
                                                .Take(pageSize)
                                                .ToListAsync();

            var result = new List<Debt>();



            if (startDate.HasValue || finishDate.HasValue)
            {
                foreach (var debt in resultFilter)
                {
                    var instalments = debt.Installments.Where(x => x.Date >= startDate && x.Date <= finishDate).ToList();
                    debt.Installments = instalments;
                    result.Add(debt);
                }

                return new PaginatedList<Debt>
                {
                    CurrentPage = pageNumber,
                    Items = result,
                    TotalItems = totalItems,
                    TotalPages = totalPages
                };
            }
            else
                return new PaginatedList<Debt>
                {
                    CurrentPage = pageNumber,
                    Items = resultFilter,
                    TotalItems = totalItems,
                    TotalPages = totalPages
                };
        }

        public async Task<Debt> GetAllByIdAsync(Guid Id)
        {
            var resultQuery = _dbSet.Include(x => x.DebtCategory).Include(p => p.Installments).Include(c => c.Card)
                                    .Where(x => x.Id == Id).FirstOrDefault();

            resultQuery.Installments = resultQuery.Installments.OrderBy(x => x.BuyDate).ToList();

            return resultQuery;
        }

        private static IQueryable<Debt> DebtsFilters(IQueryable<Debt> resultQuery,
                                                                      string name,
                                                                      decimal? value,
                                                                      DateTime? startDate,
                                                                      DateTime? finishDate,
                                                                      DebtInstallmentType? debtInstallmentType,
                                                                      DebtType? debtType,
                                                                      string category,
                                                                      bool? isGoal)
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
            if (!String.IsNullOrEmpty(category))
            {
                resultQuery = resultQuery.Where(p => p.DebtCategory.Name.ToLower().Contains(category.ToLower()));
            }
            if (isGoal.HasValue)
            {
                resultQuery = resultQuery.Where(p => p.IsGoal == isGoal.Value);
            }

            return resultQuery;
        }

        public async Task<PaginatedList<Installments>> FilterInstallmentsAsync(int pageNumber, int pageSize, Guid? debtId, Guid? cardId, int? month, int? year, DebtInstallmentType? debtInstallmentType, Status? status, DebtType? debtType, Guid userId, bool? isGoal)
        {

            var installments = _context.Installments.Include(x => x.Debt).ThenInclude(x => x.DebtCategory).AsQueryable();

            installments = installments.Where(x => x.User.Id == userId);

            if (debtId.HasValue)
                installments = installments.Where(x => x.Debt.Id == debtId.Value).AsQueryable();

            if (isGoal.HasValue)
                installments = installments.Where(x => x.Debt.IsGoal == isGoal).AsQueryable();

            if (cardId.HasValue)
                installments = installments.Where(x => x.Debt.Card.Id == cardId.Value).AsQueryable();

            if (debtInstallmentType.HasValue)
                installments = installments.Where(x => x.Debt.DebtInstallmentType == debtInstallmentType.Value);

            if (debtType.HasValue)
            {
                installments = installments.Where(x => x.Debt.DebtType == debtType.Value);
            }

            var resultQuery = InstallmentsFilters(installments, month, year, status);

            var skipNumber = pageNumber > 0 ? ((pageNumber - 1) * pageSize) : 0;
            var totalItems = resultQuery.Count();
            var totalPages = Convert.ToInt32(totalItems / pageSize + (totalItems % pageSize > 0 ? 1 : 0));

            var resultFilter = new List<Installments>();

            if (cardId.HasValue)
            {
                resultFilter = await resultQuery.OrderByDescending(x => x.BuyDate)
                                                   .ThenBy(x => x.Id)
                                                   .Skip(skipNumber)
                                                   .Take(pageSize)
                                                   .ToListAsync();
            }
            else
            {
                resultFilter = await resultQuery.OrderBy(x => x.Date)
                                                   .ThenBy(x => x.Id)
                                                   .Skip(skipNumber)
                                                   .Take(pageSize)
                                                   .ToListAsync();
            }

            return new PaginatedList<Installments>
            {
                CurrentPage = pageNumber,
                Items = resultFilter,
                TotalItems = totalItems,
                TotalPages = totalPages
            };
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

        public async Task UpdateInstallmentAsync(Guid id, Status status, DateTime? paymentDate)
        {
            var installment = await _context.Debt.SelectMany(x => x.Installments.Where(x => x.Id == id)).FirstOrDefaultAsync();

            installment.Status = status;

            if (status == Status.Paid)
            {
                installment.PaymentDate = paymentDate.Value;
            }
            else
                installment.PaymentDate = null;
        }

        public async Task<List<Installments>> GetSumPerMonthAsync(int? month, int? year, Guid userId)
        {
            var startDate = new DateTime(year.Value, month.Value, 1, 0, 0, 0);

            var futureDate = startDate.AddMonths(4);

            var finishDate = new DateTime(futureDate.Year, futureDate.Month, DateTime.DaysInMonth(futureDate.Year, futureDate.Month));


            var installments = _context.Debt.Include(x => x.Installments).SelectMany(x => x.Installments).AsQueryable();
            installments = installments.Where(x => x.User.Id == userId);
            installments = installments.Where(x => x.Date >= startDate.Date);
            installments = installments.Where(x => x.Date <= finishDate.Date);

            return await installments.ToListAsync();
        }

        public async Task<Installments> GetInstallmentById(Guid installmentId)
        {
            var installment = await _context.Debt.SelectMany(x => x.Installments.Where(x => x.Id == installmentId)).FirstOrDefaultAsync();
            return installment;
        }

        public async Task<List<Debt>> GetSumByCategoryMonth(int month, int year, Guid userId, Guid categoryId)
        {
            var query = _dbSet.Include(x => x.Installments).Include(x => x.DebtCategory).AsQueryable();
            query = query.Where(x => x.User.Id == userId);
            query = query.Where(x => x.DebtCategory.Id == categoryId);

            var response = await query.ToListAsync();
            var result = new List<Debt>();

            foreach (var debt in response)
            {
                var instalments = debt.Installments.Where(x => x.Date.Month == month && x.Date.Year <= year).ToList();
                debt.Installments = instalments;
                result.Add(debt);
            }


            return result;
        }


        public async Task EditInstallmentAsync(Installments installments)
        {
            _dbSetInstallment.Update(installments);
        }

        public async Task<List<Debt>> GetDebtResposibleParty(Guid? responsiblePartyId, int month, int year, Guid userId)
        {
            var debtResponsibleParty = _dbSet.Include(x => x.ResponsibleParty).Include(x => x.Installments).Where(x => x.ResponsibleParty != null);
            if (responsiblePartyId.HasValue)
            {
                debtResponsibleParty = debtResponsibleParty.Where(x => x.ResponsibleParty.Id == responsiblePartyId.Value);
            }

            debtResponsibleParty = debtResponsibleParty.Where(x => x.User.Id == userId);

            await debtResponsibleParty.ToListAsync();

            var result = new List<Debt>();

            foreach (var debt in debtResponsibleParty)
            {
                var instalments = debt.Installments.Where(x => x.Date.Month == month && x.Date.Year <= year).ToList();
                debt.Installments = instalments;
                result.Add(debt);
            }


            return result;
        }
    }
}
