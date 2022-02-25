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
        Task<IEnumerable<GetWalletByIdResponse>> GetWallets(WalletStatus walletStatus, int month, int year, Guid userId);
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

            wallet.Id = Guid.NewGuid(); 
            wallet.StartAt = walletAppModel.InitialDate;
            wallet.HistoryId = wallet.Id;
            wallet.User = user;

            if (walletAppModel.FinishDate.HasValue)
            {
                wallet.FinishAt = walletAppModel.FinishDate.Value;
            }

            await _walletService.CreateWalletAsync(wallet);
            await _unitOfWork.CommitAsync();

            return new GenericResponse { Id = wallet.Id };
        }

        public async Task<GenericResponse> UpdateWallet(Guid id, WalletAppModel walletAppModel)
        {
            var wallet = await _walletService.GetWalletByIdAsync(id);

            if (walletAppModel.Value != wallet.Value)
            {
                var newWallet = walletAppModel.ToEntity();
                newWallet.Id = Guid.NewGuid();
                newWallet.HistoryId = wallet.Id;
                newWallet.StartAt = walletAppModel.InitialDate;
                newWallet.User = wallet.User;
                
                if (walletAppModel.FinishDate.HasValue)
                {
                    newWallet.FinishAt = walletAppModel.FinishDate.Value;
                }

                var walletId = await _walletService.CreateWalletAsync(newWallet);

                wallet.FinishAt = DateTime.Now;
                wallet.WalletStatus = WalletStatus.Disable;

                await _walletService.UpdateWalletAsync(wallet);

                await _unitOfWork.CommitAsync();

                return new GenericResponse { Id = walletId };
            }

            wallet.Name = walletAppModel.Name;
            wallet.WalletStatus = walletAppModel.WalletStatus;

            if (walletAppModel.FinishDate.HasValue)
            {
                wallet.FinishAt = walletAppModel.FinishDate.Value;
            }

            var walletUpdate = await _walletService.UpdateWalletAsync(wallet);
            await _unitOfWork.CommitAsync();

            return new GenericResponse { Id = walletUpdate.Id };
        }

        public async Task<GetWalletByIdResponse> GetWalletById(Guid id)
        {
            var wallet = await _walletService.GetWalletByIdAsync(id);
            var walletAppResult = wallet.ToResponseModel();

            return walletAppResult;
        }

        public async Task<IEnumerable<GetWalletByIdResponse>> GetWallets(WalletStatus walletStatus, int month, int year, Guid userId)
        {
            var startDate = new DateTime(year, month, 1);
            var finishDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));

            var wallets = await _walletRepository.GetWallets(walletStatus, month, year, userId);

            var fixedWallets = wallets.Where(x => x.FinishAt == null).Select(x => x.ToResponseModel());
            var walletAppResult = wallets.Where(x => x.FinishAt > startDate && x.StartAt <= finishDate).Select(x => x.ToResponseModel());

            var walletResponse = fixedWallets.Concat(walletAppResult);

            return walletResponse;
        }

        public async Task DeleteWallet(Guid id)
        {
            await _walletService.DeleteWalletAsync(id);
        }
    }
}
