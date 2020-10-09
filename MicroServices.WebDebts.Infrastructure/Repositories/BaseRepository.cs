using MicroServices.WebDebts.Domain.Interfaces;
using MicroServices.WebDebts.Infrastructure.Database.Postgres;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MicroServices.WebDebts.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private DbSet<T> _dbSet;

        public BaseRepository(DataContext context)
        {
            _dbSet = context.Set<T>();
        }

        public Task AddAsync(T model)
        {
            throw new NotImplementedException();
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
