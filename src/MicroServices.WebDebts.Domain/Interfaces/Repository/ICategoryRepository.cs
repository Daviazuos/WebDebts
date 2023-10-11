using MicroServices.WebDebts.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroServices.WebDebts.Domain.Interfaces.Repository
{
    public interface ICategoryRepository : IBaseRepository<DebtCategory>
    {
        Task<List<DebtCategory>> GetCategories(Guid userId);
    }
}
