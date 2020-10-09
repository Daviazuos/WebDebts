using MicroServices.WebDebts.Domain.Models;
using System;
using System.Threading.Tasks;

namespace MicroServices.WebDebts.Domain.Services
{
    public interface IDebtsService
    {
        Task<bool> CreateDebt(Debt debt);
    }

    public class DebtsService : IDebtsService
    {
        public Task<bool> CreateDebt(Debt debt)
        {
            throw new NotImplementedException();
        }
    }
}
