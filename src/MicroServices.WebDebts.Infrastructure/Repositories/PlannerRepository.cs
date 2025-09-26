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

        public async Task<List<Planner>> GetByUserAndMonthAsync(Guid userId, int month, int year)
        {
            return await _context.Planners
                .Where(p => p.User != null && p.User.Id == userId && p.Month == month && p.Year == year)
                .Include(p => p.PlannerFrequencies)
                    .ThenInclude(f => f.PlannerCategories)
                        .ThenInclude(c => c.DebtCategory)
                .ToListAsync();
        }

        public async Task<PlannerCategories> FindPlannerCategoryByIdAsync(Guid plannerCategoryId)
        {
            // Busca a PlannerCategory pelo ID, incluindo a DebtCategory
            return await _context.Set<PlannerCategories>()
                .Include(pc => pc.DebtCategory)
                .FirstOrDefaultAsync(pc => pc.Id == plannerCategoryId);
        }

        public async Task UpdatePlannerCategoryAsync(PlannerCategories plannerCategory)
        {
            _context.Set<PlannerCategories>().Update(plannerCategory);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePlannerCategoryAsync(Guid plannerCategoryId)
        {
            var dbSet = _context.Set<PlannerCategories>();
            var entity = await dbSet.FindAsync(plannerCategoryId);
            if (entity != null)
            {
                dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeletePlannerFrequencyAsync(Guid plannerFrequencyId)
        {
            var dbSetFrequency = _context.Set<PlannerFrequency>();
            var dbSetCategories = _context.Set<PlannerCategories>();

            // Remover todas as categorias relacionadas à frequência
            var categories = dbSetCategories.Where(c => EF.Property<Guid>(c, "PlannerFrequencyId") == plannerFrequencyId);
            dbSetCategories.RemoveRange(categories);

            // Agora pode remover a frequência
            var frequency = await dbSetFrequency.FindAsync(plannerFrequencyId);
            if (frequency != null)
            {
                dbSetFrequency.Remove(frequency);
            }

            await _context.SaveChangesAsync();
        }
    }
}