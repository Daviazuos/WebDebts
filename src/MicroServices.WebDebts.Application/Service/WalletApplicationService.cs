using MicroServices.WebDebts.Application.Models;
using MicroServices.WebDebts.Application.Models.DebtModels;
using MicroServices.WebDebts.Application.Models.Mappers;
using MicroServices.WebDebts.Domain.Interfaces.Repository;
using MicroServices.WebDebts.Domain.Service;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using MicroServices.WebDebts.Domain.Models.Enum;

namespace MicroServices.WebDebts.Application.Service
{
    public interface IWalletApplicationService
    {
        Task<GenericResponse> CreateWallet(WalletAppModel walletAppModel, Guid userId);
        Task<GetWalletByIdResponse> GetWalletById(Guid id);
        Task<GenericResponse> UpdateWallet(Guid id, WalletAppModel walletAppModel);
        Task<IEnumerable<GetWalletByIdResponse>> GetWallets(WalletStatus? walletStatus, int month, int year, Guid userId);
        Task DeleteWallet(Guid id);
    }
    public class WalletApplicationService : IWalletApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWalletService _walletService;
        private readonly IWalletRepository _walletRepository;
        private readonly IUserRepository _userRepository;
        
        public WalletApplicationService(IUnitOfWork unitOfWork, IWalletService walletService, IUserRepository userRepository, IWalletRepository walletRepository)
        {
            _unitOfWork = unitOfWork;
            _walletService = walletService;
            _userRepository = userRepository;
            _walletRepository = walletRepository;
        }

        public async Task<GenericResponse> CreateWallet(WalletAppModel walletAppModel, Guid userId)
        {
            var user = await _userRepository.FindByIdAsync(userId);

            var wallet = walletAppModel.ToEntity();
            wallet.User = user;
            
            var id = Guid.NewGuid();
            await _walletService.CreateWalletAsync(wallet, id, userId);
            await _unitOfWork.CommitAsync();

            return new GenericResponse { Id = wallet.Id };
        }

        public async Task<GenericResponse> UpdateWallet(Guid id, WalletAppModel walletAppModel)
        {
            var wallet = await _walletService.GetWalletByIdAsync(id);

            wallet.Name = walletAppModel.Name;
            wallet.WalletStatus = walletAppModel.WalletStatus;

            foreach (var installment in wallet.WalletInstallments)
            {
                var installmentDate = new DateTime(installment.Date.Year, installment.Date.Month, 1);
                var nowDate = DateTime.UtcNow.Date;
                if (installmentDate >= new DateTime(nowDate.Date.Year, nowDate.Date.Month, 1))
                {
                    installment.Value = walletAppModel.Value;
                }
            }

            await _unitOfWork.CommitAsync();

            return new GenericResponse { Id=wallet.Id };
        }

        public async Task<GetWalletByIdResponse> GetWalletById(Guid id)
        {
            var wallet = await _walletService.GetWalletByIdAsync(id);
            var walletAppResult = wallet.ToResponseModel();

            return walletAppResult;
        }

        public async Task<IEnumerable<GetWalletByIdResponse>> GetWallets(WalletStatus? walletStatus, int month, int year, Guid userId)
        {
            var wallets = await _walletRepository.GetWallets(walletStatus, month, year, userId);

            var walletListResponse = new List<GetWalletByIdResponse>();

            foreach (var wallet in wallets)
            {
                foreach (var walletInstallment in wallet.WalletInstallments)
                {
                    walletListResponse.Add(new GetWalletByIdResponse
                    {
                        Id = wallet.Id,
                        InstallmentNumber = walletInstallment.InstallmentNumber,
                        Date = walletInstallment.Date,
                        Name = wallet.Name,
                        NumberOfInstallments = wallet.NumberOfInstallments,
                        Value = walletInstallment.Value,
                        WalletInstallmentType = wallet.WalletInstallmentType,
                        WalletStatus = wallet.WalletStatus
                    });
                }
            }

            return walletListResponse;
        }

        public async Task DeleteWallet(Guid id)
        {
            await _walletService.DeleteWalletAsync(id);
        }
    }
}
