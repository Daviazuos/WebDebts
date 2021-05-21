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
        Task<List<FilterInstallmentsResponse>> FilterInstallments(FilterInstallmentsRequest filterInstallmentsRequest);
        Task PutInstallments(PutInstallmentsRequest putInstallmentsRequest);
    }

    public class DebtsApplicationService : IDebtsApplicationService
    {
        private readonly IDebtsService _debtsServices;

        private readonly IDebtRepository _debtRepository;

        private readonly IUnitOfWork _unitOfWork;

        public DebtsApplicationService(IDebtsService debtsServices, IUnitOfWork unitOfWork, IDebtRepository debtRepository)
        {
            _debtsServices = debtsServices;
            _debtRepository = debtRepository;
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
                                                             filterDebtRequest.StartDate,
                                                             filterDebtRequest.FInishDate,
                                                             (DebtInstallmentType?)filterDebtRequest.DebtInstallmentType, 
                                                             (DebtType?)filterDebtRequest.DebtType);

            var debtAppResult = debt.Select(x => x.ToResponseModel()).ToList();

            return debtAppResult;
        }

        public async Task<List<FilterInstallmentsResponse>> FilterInstallments(FilterInstallmentsRequest filterInstallmentsRequest)
        {
            var debts = await _debtRepository.FilterInstallmentsAsync(filterInstallmentsRequest.DebtId, 
                                                                      filterInstallmentsRequest.Month, 
                                                                      filterInstallmentsRequest.Year, 
                                                                      (DebtInstallmentType?)filterInstallmentsRequest.DebtInstallmentType,
                                                                      (Status?)filterInstallmentsRequest.StatusApp);

            var installmentsApp = debts.Select( x => x.ToResponse()).OrderBy(x => x.InstallmentNumber);

            return installmentsApp.ToList();
        }

        public async Task<GetDebtByIdResponse> GetDebtsById(Guid id)
        {
            var debt = await _debtsServices.GetAllByIdAsync(id);

            var debtAppResult = debt.ToResponseModel();

            return debtAppResult;
        }

        public async Task PutInstallments(PutInstallmentsRequest putInstallmentsRequest)
        {
            await _debtRepository.UpdateInstallmentAsync(putInstallmentsRequest.Id, putInstallmentsRequest.InstallmentsStatus);
            await _unitOfWork.CommitAsync();
        }
    }
}
