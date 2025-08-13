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
    public class PlannerRepository : BaseRepository<Planner>, IPlannerRepository
    {
        private readonly DataContext _context;
        private DbSet<Planner> _dbSet;
        private readonly DbSet<PlannerFrequency> _plannerFrequencyDbSet;

        public PlannerRepository(DataContext context) : base(context)
        {
            _context = context;
            _dbSet = context.Set<Planner>();
            _plannerFrequencyDbSet = context.Set<PlannerFrequency>();
        }

        public async Task<Planner> AddAsync(Planner planner)
        {
            planner.CreatedAt = DateTime.UtcNow;
            planner.UpdatedAt = DateTime.UtcNow;
            await _dbSet.AddAsync(planner);
            await _context.SaveChangesAsync();
            return planner;
        }

        public async Task<PlannerFrequency> FindPlannerFrequencyByIdAsync(Guid plannerFrequencyId)
        {
            return await _dbSet
                .SelectMany(p => p.PlannerFrequencies)
                .Include(pf => pf.PlannerCategories)
                .ThenInclude(pc => pc.DebtCategory)
                .FirstOrDefaultAsync(pf => pf.Id == plannerFrequencyId);
        }

        public async Task<List<Planner>> GetByUserIdAsync(Guid userId)
        {
            return await _dbSet
                .Include(p => p.PlannerFrequencies)
                .ThenInclude(pf => pf.PlannerCategories)
                .ThenInclude(pc => pc.DebtCategory)
                .Include(p => p.User)
                .Where(p => p.User.Id == userId)
                .ToListAsync();
        }

        public async Task UpdatePlannerFrequencyAsync(PlannerFrequency plannerFrequency)
        {
            _plannerFrequencyDbSet.Update(plannerFrequency);
            await _context.SaveChangesAsync();
        }
    }
}