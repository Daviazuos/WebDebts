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
    public class SpendingCeilingRepository : BaseRepository<SpendingCeiling>, ISpendingCeilingRepository
    {
        private readonly DataContext _context;
        private DbSet<SpendingCeiling> _dbSet;

        public SpendingCeilingRepository(DataContext context) : base(context)
        {
            _dbSet = context.Set<SpendingCeiling>();
            _context = context;
        }

        public async Task<List<SpendingCeiling>> GetSpendingCeilingByMonth(int? month, int? year, Guid userId)
        {
            var spendingCeilings = _context.SpendingCeiling.Include(x => x.DebtCategory).AsQueryable();
            spendingCeilings = spendingCeilings.Where(x => x.User.Id == userId);
            spendingCeilings = spendingCeilings.Where(x => x.Date.Month == month);
            spendingCeilings = spendingCeilings.Where(x => x.Date.Year == year);

            return await spendingCeilings.ToListAsync();
        }
    }

}

