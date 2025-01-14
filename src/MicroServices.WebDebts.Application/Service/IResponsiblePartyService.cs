using MicroServices.WebDebts.Application.Models;
using MicroServices.WebDebts.Application.Models.DebtModels;
using MicroServices.WebDebts.Application.Models.Mappers;
using MicroServices.WebDebts.Application.Models.ResponsiblePartyModels;
using MicroServices.WebDebts.Domain.Interfaces.Repository;
using MicroServices.WebDebts.Domain.Models;
using MicroServices.WebDebts.Domain.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroServices.WebDebts.Application.Service
{
    public interface IResponsiblePartyService
    {
        Task CreateResponsibleParty(ResponsiblePartyAppModel responsiblePartyAppModel, Guid userId);
        Task LinkResponsiblePartyWDebt(Guid id, Guid? debtId, Guid? walletId);
        Task<List<ResponsiblePartyResponse>> GetResponsiblePartyByUserId(Guid userId);
    }

    public class ResponsiblePartyService : IResponsiblePartyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponsiblePartyRepository _responsiblePartyRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDebtRepository _debtRepository;
        private readonly IWalletRepository _walletRepository;

        public ResponsiblePartyService(IUnitOfWork unitOfWork, IDebtRepository debtRepository, IResponsiblePartyRepository responsiblePartyRepository, IUserRepository userRepository, IWalletRepository walletRepository)
        {
            _debtRepository = debtRepository;
            _responsiblePartyRepository = responsiblePartyRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _walletRepository = walletRepository;
        }

        public async Task CreateResponsibleParty(ResponsiblePartyAppModel responsiblePartyAppModel, Guid userId)
        {
            var user = await _userRepository.FindByIdAsync(userId);

            var responsibleParty = new ResponsibleParty
            {
                CreatedAt = DateTime.UtcNow,
                Id = Guid.NewGuid(),
                Name = responsiblePartyAppModel.Name,
                User = user,
                ImageUrl = responsiblePartyAppModel.ImageUrl
            };

            await _responsiblePartyRepository.AddAsync(responsibleParty);
            await _unitOfWork.CommitAsync();
        }

        public async Task<List<ResponsiblePartyResponse>> GetResponsiblePartyByUserId(Guid userId)
        {
            var responsableParties = await _responsiblePartyRepository.GetByUserId(userId);

            var response = new List<ResponsiblePartyResponse>();

            foreach (var responsableParty in responsableParties)
            {
                response.Add(new ResponsiblePartyResponse
                {
                    Id = responsableParty.Id,
                    ImageUrl = responsableParty.ImageUrl,
                    Name = responsableParty.Name,
                });
            }

            return response;
        }

        public async Task LinkResponsiblePartyWDebt(Guid id, Guid? debtId, Guid? walletId)
        {
            var responsibleParty = await _responsiblePartyRepository.FindByIdAsync(id);
            if (debtId.HasValue)
            {
                var debt = await _debtRepository.FindByIdAsync(debtId.Value);
                debt.ResponsibleParty = responsibleParty;
            }
            else if (walletId.HasValue)
            {
                var wallet = await _walletRepository.FindByIdAsync(walletId.Value);
                wallet.ResponsibleParty = responsibleParty;

            }
            
            await _unitOfWork.CommitAsync();
        }
    }
}