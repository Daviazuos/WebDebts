using MicroServices.WebDebts.Domain.Interfaces.Repository;
using MicroServices.WebDebts.Infrastructure.Database;
using MicroServices.WebDebts.Infrastructure.Database.Postgres;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroServices.WebDebts.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private DbSet<T> _dbSet;
        private DataContext _context;

        public BaseRepository(DataContext context)
        {
            _dbSet = context.Set<T>();
            _context = context;
        }

        public async Task AddAsync(T model)
        {
            await _context.AddAsync(model);
        }

        public Task AddManyAsync(IEnumerable<T> model)
        {
            throw new NotImplementedException();
        }

        public Task<T> FindByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task Remove(T model)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(T model)
        {
            throw new NotImplementedException();
        }
    }
}
