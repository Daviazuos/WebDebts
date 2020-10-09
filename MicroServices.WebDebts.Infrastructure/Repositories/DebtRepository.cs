using MicroServices.WebDebts.Domain.Interfaces;
using MicroServices.WebDebts.Domain.Models;
using MicroServices.WebDebts.Infrastructure.Database.Postgres;
using Microsoft.EntityFrameworkCore;

namespace MicroServices.WebDebts.Infrastructure.Repositories
{
    public class DebtRepository : BaseRepository<Debt>, IDebtsRepository
    {
        private readonly DataContext _context;
        private DbSet<Debt> _dbSet;

        public DebtRepository(DataContext context) : base(context)
        {
            _context = context;
        }
    }
}
