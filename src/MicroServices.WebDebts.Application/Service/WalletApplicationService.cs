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
using MicroServices.WebDebts.Application.Models.WalletModels;
using MicroServices.WebDebts.Domain.Models;

namespace MicroServices.WebDebts.Application.Service
{
    public interface IWalletApplicationService
    {
        Task<GenericResponse> CreateWallet(CreateWalletAppModel walletAppModel, Guid userId);
        Task<GetWalletByIdResponse> GetWalletById(Guid id);
        Task<GenericResponse> UpdateWallet(Guid id, CreateWalletAppModel walletAppModel);
        Task<IEnumerable<GetWalletByIdResponse>> GetWallets(WalletStatus? walletStatus, int month, int year, Guid userId);
        Task DeleteWallet(Guid id);
        Task<GenericResponse> UpdateWalletInstallment(Guid id, WalletInstallmentAppModel walletInstallmentAppModel);
        Task<List<GetWalletResponsiblePartiesResponse>> GetResponsiblePartiesWallets(Guid responsiblePartyId, int month, int year);
    }
    public class WalletApplicationService : IWalletApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWalletService _walletService;
        private readonly IWalletRepository _walletRepository;
        private readonly IUserRepository _userRepository;
        private readonly IResponsiblePartyRepository _responsiblePartyRepository;
        
        public WalletApplicationService(IUnitOfWork unitOfWork, IWalletService walletService, IUserRepository userRepository, IWalletRepository walletRepository, IResponsiblePartyRepository responsiblePartyRepository)
        {
            _unitOfWork = unitOfWork;
            _walletService = walletService;
            _userRepository = userRepository;
            _walletRepository = walletRepository;
            _responsiblePartyRepository = responsiblePartyRepository;
        }

        public async Task<GenericResponse> CreateWallet(CreateWalletAppModel walletAppModel, Guid userId)
        {
            var user = await _userRepository.FindByIdAsync(userId);

            var wallet = walletAppModel.ToAppEntity();
            wallet.User = user;

            if (walletAppModel.ResponsiblePartyId.HasValue)
            {
                var responsibleParty = await _responsiblePartyRepository.FindByIdAsync(walletAppModel.ResponsiblePartyId.Value);
                wallet.ResponsibleParty = responsibleParty;
            }
            else
            {
                wallet.ResponsibleParty = null;
            }
            
            var id = Guid.NewGuid();
            await _walletService.CreateWalletAsync(wallet, id, userId);
            await _unitOfWork.CommitAsync();

            return new GenericResponse { Id = wallet.Id };
        }

        public async Task<GenericResponse> UpdateWallet(Guid id, CreateWalletAppModel walletAppModel)
        {
            var wallet = await _walletService.GetWalletByIdAsync(id);

            wallet.Name = walletAppModel.Name;
            wallet.WalletStatus = walletAppModel.WalletStatus;
            wallet.Value = walletAppModel.Value;

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
                        Name = walletInstallment.Name != null? walletInstallment.Name : wallet.Name,
                        NumberOfInstallments = wallet.NumberOfInstallments,
                        Value = walletInstallment.Value,
                        ReceivedStatus = walletInstallment.ReceivedStatus,
                        InstallmentId = walletInstallment.Id,
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

        public async Task<GenericResponse> UpdateWalletInstallment(Guid id, WalletInstallmentAppModel walletInstallmentAppModel)
        {
            var walletInstallment = await _walletRepository.GetInstallmentById(id);

            walletInstallment.ReceivedStatus = (bool)walletInstallmentAppModel.ReceivedStatus;

            await _unitOfWork.CommitAsync();

            return new GenericResponse { Id = walletInstallment.Id };
        }

        public async Task<List<GetWalletResponsiblePartiesResponse>> GetResponsiblePartiesWallets(Guid responsiblePartyId, int month, int year)
        {
            var walletResponsibleParties = await _walletRepository.GetWalletResposibleParty(responsiblePartyId, month, year);

            var response = walletResponsibleParties.GroupBy(d => d.ResponsibleParty.Id)
                                    .Select(
                                        g => new GetWalletResponsiblePartiesResponse
                                        {
                                            Name = g.First().ResponsibleParty.Name,
                                            Value = g.Sum(s => s.WalletInstallments.First().Value),
                                            WalletAppModels = g.Select(x => x.ToAppModel()).ToList()
                                        }).ToList();
            return response;
        }
    }
}
