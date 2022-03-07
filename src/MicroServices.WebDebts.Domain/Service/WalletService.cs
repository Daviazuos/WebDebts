using MicroServices.WebDebts.Domain.Interfaces.Repository;
using MicroServices.WebDebts.Domain.Models;
using MicroServices.WebDebts.Domain.Models.Enum;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroServices.WebDebts.Domain.Service
{
    public interface IWalletService
    {
        Task<Guid> CreateWalletAsync(Wallet wallet);
        Task<Wallet> GetWalletByIdAsync(Guid id);
        Task<Wallet> UpdateWalletAsync(Wallet wallet);
        Task DeleteWalletAsync(Guid id);
    }
    public class WalletService : IWalletService
    {
        private readonly IWalletRepository _walletRepository;
        public WalletService(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }

        public async Task<Guid> CreateWalletAsync(Wallet wallet)
        {
            wallet.CreatedAt = DateTime.Now;

            await _walletRepository.AddAsync(wallet);

            return wallet.Id;
        }

        public async Task DeleteWalletAsync(Guid id)
        {
            var wallet = await _walletRepository.FindByIdAsync(id);
            await _walletRepository.Remove(wallet);
        }

        public async Task<Wallet> GetWalletByIdAsync(Guid id)
        {
            return await _walletRepository.FindByIdAsync(id);
        }


        public async Task<Wallet> UpdateWalletAsync(Wallet wallet)
        {
            await _walletRepository.UpdateAsync(wallet);

            return wallet;
        }
    }
}
