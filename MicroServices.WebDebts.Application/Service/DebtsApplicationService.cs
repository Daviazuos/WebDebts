using MicroServices.WebDebts.Application.Models;
using MicroServices.WebDebts.Application.Models.Mappers;
using MicroServices.WebDebts.Domain.Interfaces.Repository;
using MicroServices.WebDebts.Domain.Models.Enum;
using MicroServices.WebDebts.Domain.Services;
using System;
using System.Threading.Tasks;

namespace MicroServices.WebDebts.Application.Services
{
    public interface IDebtsApplicationService
    {
        Task<Guid> CreateDebt(DebtsAppModel createDebtsRequest);
        Task<GetDebtByIdResponse> GetDebtsById(Guid id);
        Task DeletePerson(DeleteDebtByIdRequest deletePersonRequest);
    }

    public class DebtsApplicationService : IDebtsApplicationService
    {
        private readonly IDebtsService _debtsServices;

        private readonly IUnitOfWork _unitOfWork;

        public DebtsApplicationService(IDebtsService debtsServices, IUnitOfWork unitOfWork, IDebtRepository debtRepository)
        {
            _debtsServices = debtsServices;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> CreateDebt(DebtsAppModel createDebtsRequest)
        {
            var debt = createDebtsRequest.ToEntity();
            


            await _debtsServices.CreateDebtAsync(debt, DebtType.Simple);
            await _unitOfWork.CommitAsync();
            
            return debt.Id;
        }

        public async Task DeletePerson(DeleteDebtByIdRequest deleteDebtByIdRequest)
        {
            await _debtsServices.DeleteDebt(deleteDebtByIdRequest.Id);
            await _unitOfWork.CommitAsync();
        }

        public async Task<GetDebtByIdResponse> GetDebtsById(Guid id)
        {
            var debt = await _debtsServices.GetAllByIdAsync(id);

            var debtAppResult = debt.ToResponseModel();

            return debtAppResult;
        }
    }
}
