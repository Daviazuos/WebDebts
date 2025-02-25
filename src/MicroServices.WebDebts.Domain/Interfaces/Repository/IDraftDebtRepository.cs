using MicroServices.WebDebts.Domain.Models;
using MicroServices.WebDebts.Domain.Models.Commom;
using MicroServices.WebDebts.Domain.Models.Enum;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroServices.WebDebts.Domain.Interfaces.Repository
{
    public interface IDraftDebtRepository : IBaseRepository<DraftDebt>
    {
        Task<List<DraftDebt>> GetByUserIdAsync(Guid userId);
    }
}
