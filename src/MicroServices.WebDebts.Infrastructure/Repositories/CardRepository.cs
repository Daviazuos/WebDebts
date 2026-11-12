using MicroServices.WebDebts.Domain.Interfaces.Repository;
using MicroServices.WebDebts.Domain.Models;
using MicroServices.WebDebts.Domain.Models.Commom;
using MicroServices.WebDebts.Domain.Models.Enum;
using MicroServices.WebDebts.Infrastructure.Database.Postgres;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroServices.WebDebts.Infrastructure.Repositories
{
    public class CardRepository : BaseRepository<Card>, ICardRepository
    {
        private readonly DataContext _context;
        private DbSet<Card> _dbSet;

        public CardRepository(DataContext context) : base(context)
        {
            _dbSet = context.Set<Card>();
            _context = context;
        }

        public async Task<List<Debt>> FilterCardsAsync(int? month, int? year)
        {
            var resultQuery = _dbSet.Include(x => x.DebtValues).SelectMany(x => x.DebtValues);

            if (month.HasValue && year.HasValue)
            {
                resultQuery = resultQuery.Where(x => x.Date.Month == month.Value && x.Date.Year == year.Value);
            }

            var resultDebts = await resultQuery.ToListAsync();

            return resultDebts;
        }

        public async Task<PaginatedList<Card>> FindCardValuesByIdAsync(int pageNumber, int pageSize, Guid? id, Guid userId, int? month, int? year, bool withNoDebts)
        {
            var resultQuery = _dbSet.Where(x => x.User.Id == userId);

            if (id.HasValue)
            {
                resultQuery = resultQuery.Where(x => x.Id == id.Value);
            }

            resultQuery = resultQuery.Include(x => x.DebtValues)
                                     .ThenInclude(x => x.Installments);

            if (month.HasValue && year.HasValue && !withNoDebts)
            {
                resultQuery = resultQuery.Where(x =>
                    x.DebtValues.Any(dv =>
                        dv.Installments.Any(inst =>
                            inst.Date.Month == month.Value && inst.Date.Year == year.Value
                        )
                    )
                );
            }

            var skipNumber = pageNumber > 0 ? ((pageNumber - 1) * pageSize) : 0;
            var totalItems = resultQuery.Count();
            var totalPages = Convert.ToInt32(totalItems / pageSize + (totalItems % pageSize > 0 ? 1 : 0));

            var resultFilter = await resultQuery.Skip(skipNumber)
                                                .Take(pageSize)
                                                .ToListAsync();

            return new PaginatedList<Card>
            {
                CurrentPage = pageNumber,
                Items = resultFilter,
                TotalItems = totalItems,
                TotalPages = totalPages
            };
        }

        public async Task<PaginatedList<CardMonthlySummary>> GetMonthlyCardSummariesAsync(int pageNumber, int pageSize, int month, int year, Guid userId, bool withNoDebts, bool? isPaid)
        {
            var monthStart = new DateTime(year, month, 1);
            var nextMonthStart = monthStart.AddMonths(1);

            var cardsQuery = _dbSet.Where(card => card.User.Id == userId);

            if (withNoDebts && !isPaid.HasValue)
            {
                var allCardsTotalItems = await cardsQuery.CountAsync();
                var allCardsPageItems = await cardsQuery
                    .OrderBy(card => card.Name)
                    .ThenBy(card => card.Id)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(card => new CardMonthlySummary
                    {
                        Id = card.Id,
                        Name = card.Name,
                        DueDate = card.DueDate,
                        ClosureDate = card.ClosureDate,
                        Color = card.Color
                    })
                    .ToListAsync();

                var pageCardIds = allCardsPageItems.Select(card => card.Id).ToList();
                var pageTotals = await _context.Set<Installments>()
                    .Where(installment =>
                        installment.Date >= monthStart &&
                        installment.Date < nextMonthStart &&
                        pageCardIds.Contains(installment.Debt.Card.Id))
                    .GroupBy(installment => installment.Debt.Card.Id)
                    .Select(group => new
                    {
                        CardId = group.Key,
                        TotalValue = group.Sum(installment => installment.Value),
                        InstallmentCount = group.Count(),
                        PaidInstallmentCount = group.Count(installment => installment.Status == Status.Paid)
                    })
                    .ToListAsync();
                var totalsByCardId = pageTotals.ToDictionary(totals => totals.CardId);

                foreach (var card in allCardsPageItems)
                {
                    if (!totalsByCardId.TryGetValue(card.Id, out var totals))
                        continue;

                    card.TotalValue = totals.TotalValue;
                    card.HasMonthlyInstallments = true;
                    card.IsPaid = totals.PaidInstallmentCount == totals.InstallmentCount;
                }

                return new PaginatedList<CardMonthlySummary>
                {
                    CurrentPage = pageNumber,
                    TotalPages = Convert.ToInt32(allCardsTotalItems / pageSize + (allCardsTotalItems % pageSize > 0 ? 1 : 0)),
                    TotalItems = allCardsTotalItems,
                    Items = allCardsPageItems
                };
            }

            var monthlyTotals = _context.Set<Installments>()
                .Where(installment =>
                    installment.Date >= monthStart &&
                    installment.Date < nextMonthStart &&
                    installment.Debt.Card.User.Id == userId)
                .GroupBy(installment => installment.Debt.Card.Id)
                .Select(group => new
                {
                    CardId = group.Key,
                    TotalValue = group.Sum(installment => installment.Value),
                    InstallmentCount = group.Count(),
                    PaidInstallmentCount = group.Count(installment => installment.Status == Status.Paid)
                });

            var query =
                from totals in monthlyTotals
                join card in cardsQuery on totals.CardId equals card.Id
                select new CardMonthlySummary
                {
                    Id = card.Id,
                    Name = card.Name,
                    DueDate = card.DueDate,
                    ClosureDate = card.ClosureDate,
                    Color = card.Color,
                    TotalValue = totals.TotalValue,
                    HasMonthlyInstallments = true,
                    IsPaid = totals.PaidInstallmentCount == totals.InstallmentCount
                };

            if (isPaid.HasValue)
            {
                query = query.Where(card => card.IsPaid == isPaid.Value);
            }

            var totalItems = await query.CountAsync();
            var totalPages = Convert.ToInt32(totalItems / pageSize + (totalItems % pageSize > 0 ? 1 : 0));
            var items = await query
                .OrderBy(card => card.Name)
                .ThenBy(card => card.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedList<CardMonthlySummary>
            {
                CurrentPage = pageNumber,
                TotalPages = totalPages,
                TotalItems = totalItems,
                Items = items
            };
        }

        public async Task<Card> GetCardById(Guid id)
        {
            var card = await _dbSet.Include(x => x.DebtValues).ThenInclude(x => x.Installments).FirstAsync(x => x.Id == id);
            return card;
        }

        public async Task<Card> GetCardByName(string cardName, Guid userId)
        {
            return _dbSet.Where(x => x.Name == cardName).FirstOrDefault(x => x.User.Id == userId);
        }
    }

}

