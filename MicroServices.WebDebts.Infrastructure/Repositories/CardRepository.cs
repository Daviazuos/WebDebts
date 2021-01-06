using MicroServices.WebDebts.Domain.Interfaces.Repository;
using MicroServices.WebDebts.Domain.Models;
using MicroServices.WebDebts.Infrastructure.Database.Postgres;
using Microsoft.EntityFrameworkCore;
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

        public async Task<Card> GetCardByName(string cardName)
        {
            return _dbSet.Where(x => x.Name == cardName).FirstOrDefault();
        }
    }

}

