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
    public class CategoryRepository : BaseRepository<DebtCategory>, ICategoryRepository
    {
        private readonly DataContext _context;
        private DbSet<DebtCategory> _dbSet;

        public CategoryRepository(DataContext context) : base(context)
        {
            _dbSet = context.Set<DebtCategory>();
            _context = context;
        }

        public async Task<List<DebtCategory>> GetCategories(Guid userId)
        {
            var categories = await _dbSet.Where(x => x.User.Id == userId).ToListAsync();

            return categories;
        }
    }

}

