using MicroServices.WebDebts.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroServices.WebDebts.Domain.Interfaces.Repository
{
    public interface IWalletRepository : IBaseRepository<Wallet>
    {
        Task<List<Wallet>> GetEnableWallet();
        Task<List<Wallet>> GetWalletByMonth(int? month, int? year);
    }
}
