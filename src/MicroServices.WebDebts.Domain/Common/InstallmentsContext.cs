using MicroServices.WebDebts.Domain.Models;
using System.Collections.Generic;
using static MicroServices.WebDebts.Domain.Service.InstallmentsStrategy;

namespace MicroServices.WebDebts.Domain.Common
{
    public class InstallmentsContext
    {
        private IInstallmentsStrategy _InstallmentsStrategy;

        public InstallmentsContext()
        {

        }

        public InstallmentsContext(IInstallmentsStrategy installmentsStrategy)
        {
            _InstallmentsStrategy = installmentsStrategy;
        }

        public void SetStrategy(IInstallmentsStrategy installmentsStrategy)
        {
            _InstallmentsStrategy = installmentsStrategy;
        }

        public List<Installments> CreateInstallments(Debt debt, User user)
        {
            return _InstallmentsStrategy.CreateInstallmentsStrategy(debt, user);
        }
    }
}
