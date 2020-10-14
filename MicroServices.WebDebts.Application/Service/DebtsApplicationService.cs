using MicroServices.WebDebts.Application.Models;
using MicroServices.WebDebts.Application.Models.Mappers;
using MicroServices.WebDebts.Domain.Interfaces.Repository;
using MicroServices.WebDebts.Domain.Models;
using MicroServices.WebDebts.Domain.Models.Enum;
using MicroServices.WebDebts.Domain.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroServices.WebDebts.Application.Services
{
    public interface IDebtsApplicationService
    {
        Task<bool> CreateSimpleDebt(DebtsAppModel createDebtsRequest);
    }

    public class DebtsApplicationService : IDebtsApplicationService
    {
        private readonly IDebtsService _debtsServices;

        private readonly IUnitOfWork _unitOfWork;

        public DebtsApplicationService(IDebtsService debtsServices, IUnitOfWork unitOfWork)
        {
            _debtsServices = debtsServices;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CreateSimpleDebt(DebtsAppModel createDebtsRequest)
        {
            var installments = CreateInstallments(createDebtsRequest);

            var debt = createDebtsRequest.ToEntity();

            debt.Installments = installments;
            debt.Id = Guid.NewGuid();

            await _debtsServices.CreateDebt(debt);
            await _unitOfWork.CommitAsync();
            return true;
        }

        public static List<Installments> CreateInstallments(DebtsAppModel debts)
        {
            var installmentsList = new List<Installments>();
            var installmentValue = debts.Value / debts.NumberOfInstallments;

            for (int i = 0; i < debts.NumberOfInstallments; i++)
            {
                var installment = new Installments
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.Now,
                    InstallmentNumber = i + 1,
                    Date = debts.Date.AddMonths(i),
                    Status = Status.NotPaid,
                    PaymentDate = null,
                    Value = installmentValue
                };

                installmentsList.Add(installment);
            }

            return installmentsList;
        }
    }
}
