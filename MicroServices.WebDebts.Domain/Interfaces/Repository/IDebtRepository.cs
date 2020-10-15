using MicroServices.WebDebts.Domain.Models;
using System;
using System.Threading.Tasks;

namespace MicroServices.WebDebts.Domain.Interfaces.Repository
{
    public interface IDebtRepository : IBaseRepository<Debt>
    {
        Task<Debt> GetAllByIdAsync(Guid Id);
        Task DeleteDebt(Guid Id);
    }
}
