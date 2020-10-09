using MicroServices.WebDebts.Application.Models;
using MicroServices.WebDebts.Domain.Models;
using MicroServices.WebDebts.Domain.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroServices.WebDebts.Application.Services
{
    public interface IDebtsApplicationService
    {
        Task<bool> CreateSimpleDebt(CreateDebtsRequest createDebtsRequest);
    }

    public class DebtsApplicationService : IDebtsApplicationService
    {
        private readonly IDebtsService _debtsServices;

        public DebtsApplicationService(IDebtsService debtsServices)
        {
            _debtsServices = debtsServices;
        }

        public async Task<bool> CreateSimpleDebt(CreateDebtsRequest createDebtsRequest)
        {
            var installments = CreateInstallments(createDebtsRequest);

            var debt = Debt.Create(createDebtsRequest.Name, createDebtsRequest.Value, createDebtsRequest.DebtType, createDebtsRequest.Status, installments);

            await _debtsServices.CreateDebt(debt);

            return true;
        }

        public static List<Installments> CreateInstallments(CreateDebtsRequest debts)
        {
            var installmentsList = new List<Installments>();
            var installmentValue = debts.Value / debts.NumberOfInstallments;

            for (int i = 1; i < debts.NumberOfInstallments; i++)
            {
                var installment = new Installments
                {
                    InstallmentNumber = i,
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
