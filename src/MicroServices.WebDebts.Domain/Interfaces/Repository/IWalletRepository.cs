using MicroServices.WebDebts.Domain.Models;
using MicroServices.WebDebts.Domain.Models.Enum;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroServices.WebDebts.Domain.Interfaces.Repository
{
    public interface IWalletRepository : IBaseRepository<Wallet>
    {
        Task<List<Wallet>> GetWallets(WalletStatus? walletStatus, int month, int year, Guid userId);
        Task<List<Wallet>> GetWalletByMonth(int? month, int? year, Guid userId);

        Task<List<WalletInstallments>> GetWalletInstallmentByMonth(int? month, int? year, Guid userId);
        Task<Wallet> GetWalletByIdAsync(Guid id);
    }
}
