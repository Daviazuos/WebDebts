using MicroServices.WebDebts.Domain.Interfaces;
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
        private readonly IDebtsRepository _debtRepository;

        public DebtsService(IDebtsRepository debtRepository)
        {
            _debtRepository = debtRepository;
        }

        public async Task CreateDebt(Debt debt)
        {
            await _debtRepository.AddAsync(debt);
        }
    }
}
