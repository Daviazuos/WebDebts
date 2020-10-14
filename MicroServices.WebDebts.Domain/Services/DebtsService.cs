using MicroServices.WebDebts.Domain.Interfaces.Repository;
using MicroServices.WebDebts.Domain.Models;
using System.Threading.Tasks;

namespace MicroServices.WebDebts.Domain.Services
{
    public interface IDebtsService
    {
        Task CreateDebt(Debt debt);
    }

    public class DebtsService : IDebtsService
    {
        private readonly IDebtRepository _debtRepository;
        public DebtsService(IDebtRepository debtRepository)
        {
            _debtRepository = debtRepository;
        }

        public async Task CreateDebt(Debt debt)
        {
            await _debtRepository.AddAsync(debt);
        }
    }
}
