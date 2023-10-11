using MicroServices.WebDebts.Domain.Models;
using System.Collections.Generic;
using static MicroServices.WebDebts.Domain.Service.WalletInstallmentsStrategy;

namespace MicroServices.WebDebts.Domain.Common
{
    public class WalletInstallmentsContext
    {
        private IWalletInstallmentsStrategy _WalletInstallmentsStrategy;

        public WalletInstallmentsContext()
        {

        }

        public WalletInstallmentsContext(IWalletInstallmentsStrategy walletInstallmentsStrategy)
        {
            _WalletInstallmentsStrategy = walletInstallmentsStrategy;
        }

        public void SetStrategy(IWalletInstallmentsStrategy walletInstallmentsStrategy)
        {
            _WalletInstallmentsStrategy = walletInstallmentsStrategy;
        }

        public List<WalletInstallments> CreateWalletInstallments(Wallet wallet, User user)
        {
            return _WalletInstallmentsStrategy.CreateInstallmentsStrategy(wallet, user);
        }
    }
}
