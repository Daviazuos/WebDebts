using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroServices.WebDebts.Domain.Interfaces.Repository
{
    public interface IBaseRepository<T> where T : class
    {
        Task AddAsync(T model);

        Task AddManyAsync(IEnumerable<T> model);

        Task UpdateAsync(T model);

        Task Remove(T model);

        Task<T> FindByIdAsync(Guid id);
    }
}
