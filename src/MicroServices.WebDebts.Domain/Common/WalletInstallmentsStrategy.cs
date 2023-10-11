using MicroServices.WebDebts.Domain.Models;
using MicroServices.WebDebts.Domain.Models.Enum;
using System;
using System.Collections.Generic;

namespace MicroServices.WebDebts.Domain.Service
{
    public class WalletInstallmentsStrategy
    {
        public interface IWalletInstallmentsStrategy
        {
            List<WalletInstallments> CreateInstallmentsStrategy(Wallet wallet, User user);
        }

        public class CreateInstallments : IWalletInstallmentsStrategy
        {
            public List<WalletInstallments> CreateInstallmentsStrategy(Wallet wallet, User user)
            {
                var installmentsList = new List<WalletInstallments>();

                for (int i = 0; i < wallet.NumberOfInstallments; i++)
                {
                    var installment = new WalletInstallments
                    {
                        Id = Guid.NewGuid(),
                        CreatedAt = DateTime.Now,
                        InstallmentNumber = i + 1,
                        Date = wallet.Date.AddMonths(i),
                        Value = wallet.Value,
                        User = user
                    };
                    
                    installmentsList.Add(installment);
                }

                return installmentsList;
            }
        }

        public class CreateFixedInstallments : IWalletInstallmentsStrategy
        {
            public List<WalletInstallments> CreateInstallmentsStrategy(Wallet wallet, User user)
            {
                // Todo Criando as parcelas fixas com um total fixo de 5 anos 

                var installmentsList = new List<WalletInstallments>();

                for (int i = 0; i < 120; i++)
                {
                    var installment = new WalletInstallments
                    {
                        Id = Guid.NewGuid(),
                        CreatedAt = DateTime.Now,
                        InstallmentNumber = 0,
                        Date = wallet.Date.AddMonths(i),
                        Value = wallet.Value,
                        User = user
                    };
                    
                    

                    installmentsList.Add(installment);
                }

                return installmentsList;
            }
        }

        public class CreateSimpleInstallments : IWalletInstallmentsStrategy
        {
            public List<WalletInstallments> CreateInstallmentsStrategy(Wallet wallet, User user)
            {
                var installmentsList = new List<WalletInstallments>();
                var installment = new WalletInstallments
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.Now,
                    InstallmentNumber = 1,
                    Date = wallet.Date,
                    Value = wallet.Value,
                    User = user
                };

                installmentsList.Add(installment);

                return installmentsList;
            }
        }
    }
}
