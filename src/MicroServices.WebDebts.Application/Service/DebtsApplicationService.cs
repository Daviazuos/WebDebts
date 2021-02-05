using MicroServices.WebDebts.Application.Models;
using MicroServices.WebDebts.Application.Models.DebtModels;
using MicroServices.WebDebts.Application.Models.Mappers;
using MicroServices.WebDebts.Domain.Interfaces.Repository;
using MicroServices.WebDebts.Domain.Models.Enum;
using MicroServices.WebDebts.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroServices.WebDebts.Application.Services
{
    public interface IDebtsApplicationService
    {
        Task<GenericResponse> CreateDebt(DebtsAppModel createDebtsRequest);
        Task<GetDebtByIdResponse> GetDebtsById(Guid id);
        Task DeletePerson(DeleteDebtByIdRequest deletePersonRequest);
        Task<List<GetDebtByIdResponse>> FilterDebtsById(FilterDebtRequest filterDebtRequest);
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

        public async Task<GenericResponse> CreateDebt(DebtsAppModel createDebtsRequest)
        {
            var debt = createDebtsRequest.ToEntity();
            
            await _debtsServices.CreateDebtAsync(debt, DebtType.Simple);
            await _unitOfWork.CommitAsync();
            
            return new GenericResponse { Id = debt.Id };
        }

        public async Task DeletePerson(DeleteDebtByIdRequest deleteDebtByIdRequest)
        {
            await _debtsServices.DeleteDebt(deleteDebtByIdRequest.Id);
            await _unitOfWork.CommitAsync();
        }

        public async Task<List<GetDebtByIdResponse>> FilterDebtsById(FilterDebtRequest filterDebtRequest)
        {
            var debt = await _debtsServices.FilterDebtsAsync(filterDebtRequest.Name, 
                                                             filterDebtRequest.Value, 
                                                             filterDebtRequest.Date, 
                                                             (DebtInstallmentType?)filterDebtRequest.DebtInstallmentType, 
                                                             (DebtType?)filterDebtRequest.DebtType);

            var debtAppResult = debt.Select(x => x.ToResponseModel()).ToList();

            return debtAppResult;
        }

        public async Task<GetDebtByIdResponse> GetDebtsById(Guid id)
        {
            var debt = await _debtsServices.GetAllByIdAsync(id);

            var debtAppResult = debt.ToResponseModel();

            return debtAppResult;
        }
    }
}
