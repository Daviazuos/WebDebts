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
    public class GoalRepository : BaseRepository<Goal>, IGoalRepository
    {
        private readonly DataContext _context;
        private DbSet<Goal> _dbSet;

        public GoalRepository(DataContext context) : base(context)
        {
            _dbSet = context.Set<Goal>();
            _context = context;
        }

        public async Task<List<Goal>> FilterGoalAsync(Guid? id, Guid userId)
        {
            var goal = await _dbSet.Include(x => x.Debt).ThenInclude(x => x.Installments).Where(x => x.User.Id == userId).ToListAsync();
            return goal;
        }

        public async Task<Goal> GetGoalById(Guid id)
        {
            var goal = await _dbSet.Include(x => x.Debt).ThenInclude(x => x.Installments).FirstAsync(x => x.Id == id);
            return goal;
        }
    }

}

