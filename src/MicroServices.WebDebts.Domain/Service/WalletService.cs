using MicroServices.WebDebts.Domain.Common;
using MicroServices.WebDebts.Domain.Interfaces.Repository;
using MicroServices.WebDebts.Domain.Models;
using MicroServices.WebDebts.Domain.Models.Enum;
using System;
using System.Threading.Tasks;
using static MicroServices.WebDebts.Domain.Service.WalletInstallmentsStrategy;

namespace MicroServices.WebDebts.Domain.Service
{
    public interface IWalletService
    {
        Task<Guid> CreateWalletAsync(Wallet wallet, Guid id, Guid userId);
        Task<Wallet> GetWalletByIdAsync(Guid id);
        Task<Wallet> UpdateWalletAsync(Wallet wallet);
        Task DeleteWalletAsync(Guid id);
    }
    public class WalletService : IWalletService
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public WalletService(IWalletRepository walletRepository, IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _walletRepository = walletRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> CreateWalletAsync(Wallet wallet, Guid id, Guid userId)
        {
            var user = await _userRepository.FindByIdAsync(userId);

            var classInstallments = new WalletInstallmentsContext();

            if (wallet.WalletInstallmentType == WalletInstallmentType.Simple)
                classInstallments.SetStrategy(new CreateSimpleInstallments());
            else if (wallet.WalletInstallmentType == WalletInstallmentType.Fixed)
                classInstallments.SetStrategy(new CreateFixedInstallments());
            else if (wallet.WalletInstallmentType == WalletInstallmentType.Installment)
            {
                classInstallments.SetStrategy(new CreateInstallments());
            }

            var installments = classInstallments.CreateWalletInstallments(wallet, user);

            wallet.WalletInstallments = installments;
            wallet.Id = id;
            wallet.CreatedAt = DateTime.Now;
            
            await _walletRepository.AddAsync(wallet);

            return id;
        }

        public async Task DeleteWalletAsync(Guid id)
        {
            var wallet = await _walletRepository.GetWalletByIdAsync(id);
            wallet.WalletStatus = WalletStatus.Disable;

            await _unitOfWork.CommitAsync();
        }

        public async Task<Wallet> GetWalletByIdAsync(Guid id)
        {
            return await _walletRepository.GetWalletByIdAsync(id);
        }


        public async Task<Wallet> UpdateWalletAsync(Wallet wallet)
        {
            await _walletRepository.UpdateAsync(wallet);

            return wallet;
        }
    }
}
