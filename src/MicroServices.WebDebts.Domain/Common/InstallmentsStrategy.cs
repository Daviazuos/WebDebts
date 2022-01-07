using MicroServices.WebDebts.Domain.Models;
using MicroServices.WebDebts.Domain.Models.Enum;
using System;
using System.Collections.Generic;

namespace MicroServices.WebDebts.Domain.Service
{
    public class InstallmentsStrategy
    {
        public interface IInstallmentsStrategy
        {
            List<Installments> CreateInstallmentsStrategy(Debt debt);
        }

        public class CreateInstallments : IInstallmentsStrategy
        {
            public List<Installments> CreateInstallmentsStrategy(Debt debt)
            {
                var installmentsList = new List<Installments>();

                for (int i = 0; i < debt.NumberOfInstallments; i++)
                {
                    var installment = new Installments();
                    var installmentValue = debt.Value / debt.NumberOfInstallments;
                    var date = debt.Date.AddMonths(i);

                    if (date < new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1))
                    {
                        installment = new Installments
                        {
                            Id = Guid.NewGuid(),
                            CreatedAt = DateTime.Now,
                            InstallmentNumber = i + 1,
                            Date = date,
                            Status = Status.Paid,
                            PaymentDate = date,
                            Value = installmentValue
                        };
                    }
                    else
                    {
                        installment = new Installments
                        {
                            Id = Guid.NewGuid(),
                            CreatedAt = DateTime.Now,
                            InstallmentNumber = i + 1,
                            Date = debt.Date.AddMonths(i),
                            Status = Status.NotPaid,
                            PaymentDate = null,
                            Value = installmentValue
                        };
                    }

                    installmentsList.Add(installment);
                }

                return installmentsList;
            }
        }

        public class CreateFixedInstallments : IInstallmentsStrategy
        {
            public List<Installments> CreateInstallmentsStrategy(Debt debt)
            {
                // Todo Criando as parcelas fixas com um total fixo de 5 anos 

                var installmentsList = new List<Installments>();

                for (int i = 0; i < 120; i++)
                {
                    var installment = new Installments();
                    var date = debt.Date.AddMonths(i);
                    if (date < new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1))
                    {
                        installment = new Installments
                        {
                            Id = Guid.NewGuid(),
                            CreatedAt = DateTime.Now,
                            InstallmentNumber = 0,
                            Date = date,
                            Status = Status.Paid,
                            PaymentDate = date,
                            Value = debt.Value
                        };
                    }
                    else
                    {
                        installment = new Installments
                        {
                            Id = Guid.NewGuid(),
                            CreatedAt = DateTime.Now,
                            InstallmentNumber = 0,
                            Date = debt.Date.AddMonths(i),
                            Status = Status.NotPaid,
                            PaymentDate = null,
                            Value = debt.Value
                        };
                    }
                    

                    installmentsList.Add(installment);
                }

                return installmentsList;
            }
        }

        public class CreateSimpleInstallments : IInstallmentsStrategy
        {
            public List<Installments> CreateInstallmentsStrategy(Debt debt)
            {
                var installmentsList = new List<Installments>();
                var installment = new Installments
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.Now,
                    InstallmentNumber = 1,
                    Date = debt.Date,
                    Status = Status.NotPaid,
                    PaymentDate = null,
                    Value = debt.Value
                };

                installmentsList.Add(installment);

                return installmentsList;
            }
        }
    }
}
