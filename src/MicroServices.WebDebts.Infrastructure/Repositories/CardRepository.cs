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

        public async Task<List<Card>> FindCardValuesByIdAsync(Guid? id, Guid userId)
        {
            var resultFilter = _dbSet.Include(x => x.DebtValues)
                                     .ThenInclude(x => x.Installments).Where(x => x.User.Id == userId).Select(x => x);

            if (id.HasValue)
            {   
                resultFilter = resultFilter.Where(x => x.Id == id.Value);
            }

            return await resultFilter.ToListAsync();
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

