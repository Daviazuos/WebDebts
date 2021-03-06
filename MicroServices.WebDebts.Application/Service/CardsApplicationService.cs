﻿using MicroServices.WebDebts.Application.Models;
using MicroServices.WebDebts.Application.Models.DebtModels;
using MicroServices.WebDebts.Application.Models.Mappers;
using MicroServices.WebDebts.Domain.Interfaces.Repository;
using MicroServices.WebDebts.Domain.Models.Enum;
using MicroServices.WebDebts.Domain.Service;
using MicroServices.WebDebts.Domain.Services;
using System;
using System.Threading.Tasks;

namespace MicroServices.WebDebts.Application.Service
{
    public interface ICardsApplicationService
    {
        Task<GenericResponse> CreateCard(CardAppModel cardAppModel);
        Task<GenericResponse> AddValuesCard(DebtsAppModel debtsAppModel, string CardName);
        Task<GetCardByIdResponse> GetCardById(Guid id);
        Task<GetCardByIdResponse> GetCardValuesById(Guid id);
    }
    public class CardsApplicationService : ICardsApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDebtsService _debtsService;
        private readonly ICardService _cardService;

        public CardsApplicationService(IUnitOfWork unitOfWork, IDebtsService debtsService, ICardService cardService)
        {
            _unitOfWork = unitOfWork;
            _cardService = cardService;
            _debtsService = debtsService;
        }

        public async Task<GenericResponse> AddValuesCard(DebtsAppModel debtsAppModel, string CardName)
        {
            var debt = debtsAppModel.ToEntity();
            var debtCard = await _cardService.LinkCard(debt, CardName);

            await _debtsService.CreateDebtAsync(debtCard, DebtType.Card);
            await _unitOfWork.CommitAsync();

            return new GenericResponse { Id = debt.Id };
        }

        public async Task<GenericResponse> CreateCard(CardAppModel cardAppModel)
        {
            var card = cardAppModel.ToEntity();
            var cardId = await _cardService.CreateCardAsync(card);
            await _unitOfWork.CommitAsync();

            return new GenericResponse { Id = cardId };
        }

        public async Task<GetCardByIdResponse> GetCardById(Guid id)
        {
            var card = await _cardService.GetAllByIdAsync(id);

            var cardAppResult = card.ToResponseModel();

            return cardAppResult;
        }

        public async Task<GetCardByIdResponse> GetCardValuesById(Guid id)
        {
            var card = await _cardService.GetAllCardValuesByIdAsync(id);

            var cardAppResult = card.ToResponseModel();

            return cardAppResult;
        }
    }
}
