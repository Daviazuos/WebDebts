using MicroServices.WebDebts.Domain.Models;
using MicroServices.WebDebts.Domain.Models.Enum;
using System;
using System.Collections.Generic;

namespace MicroServices.WebDebts.Domain.Service
{
    public class CreateDebtsInstallments
    {
        public List<Installments> CreateInstallmentsMethod(Debt debt)
        {
            // todo ver factory e strategy
            var installments = new List<Installments>();

            if (debt.DebtInstallmentType == DebtInstallmentType.Fixed)
            {
                installments = CreateFixedinstallment(debt);
                debt.NumberOfInstallments = 0;
            }
            else if (debt.DebtInstallmentType == DebtInstallmentType.Installment)
            {
                installments = CreateInstallments(debt);
            }
            else if (debt.DebtInstallmentType == DebtInstallmentType.Simple)
            {
                installments = CreateSimpleInstallments(debt);
                debt.NumberOfInstallments = 1;
            }

            return installments;
        }

        private List<Installments> CreateInstallments(Debt debts)
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

        private List<Installments> CreateFixedinstallment(Debt debts)
        {
            // Criando as parcelas fixas com um total fixo de 5 anos 

            var installmentsList = new List<Installments>();
            
            for (int i = 0; i < 120; i++)
            {
                var installment = new Installments
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.Now,
                    InstallmentNumber = 0,
                    Date = debts.Date.AddMonths(i),
                    Status = Status.NotPaid,
                    PaymentDate = null,
                    Value = debts.Value
                };

                installmentsList.Add(installment);
            }

            return installmentsList;
        }

        private List<Installments> CreateSimpleInstallments(Debt debts)
        {
            var installmentsList = new List<Installments>();
            var installment = new Installments
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.Now,
                InstallmentNumber = 1,
                Date = debts.Date,
                Status = Status.NotPaid,
                PaymentDate = null,
                Value = debts.Value
            };

            installmentsList.Add(installment);

            return installmentsList;
        }
    }
}
