using MicroServices.WebDebts.Domain.Interfaces.Repository;
using MicroServices.WebDebts.Domain.Models;
using MicroServices.WebDebts.Domain.Models.Commom;
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

            if (!withNoDebts)
            {
                resultQuery = resultQuery.Include(x => x.DebtValues)
                                     .ThenInclude(x => x.Installments).Where(x =>
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

