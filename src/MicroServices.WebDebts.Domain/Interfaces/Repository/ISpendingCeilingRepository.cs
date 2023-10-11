using MicroServices.WebDebts.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroServices.WebDebts.Domain.Interfaces.Repository
{
    public interface ISpendingCeilingRepository : IBaseRepository<SpendingCeiling>
    {
        Task<List<SpendingCeiling>> GetSpendingCeilingByMonth(int? month, int? year, Guid userId);
    }
}
