using MicroServices.WebDebts.Application.Models;
using MicroServices.WebDebts.Application.Models.DebtModels;
using MicroServices.WebDebts.Application.Models.Mappers;
using MicroServices.WebDebts.Domain.Interfaces.Repository;
using MicroServices.WebDebts.Domain.Models.Enum;
using MicroServices.WebDebts.Domain.Service;
using MicroServices.WebDebts.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroServices.WebDebts.Application.Service
{
    public interface ICardsApplicationService
    {
        Task<GenericResponse> CreateCard(CardAppModel cardAppModel);
        Task<GenericResponse> AddValuesCard(CreateDebtAppModel createDebtAppModel, Guid cardId);
        Task<GetCardsResponse> GetCardById(Guid id);
        Task<List<GetCardsResponse>> FilterCardsAsync(Guid? id, int? month, int? year);
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

        public async Task<GenericResponse> AddValuesCard(CreateDebtAppModel debtsAppModel, Guid cardId)
        {
            var debt = debtsAppModel.ToCreateModel();
            var debtCard = await _cardService.LinkCard(debt, cardId);

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

        public async Task<GetCardsResponse> GetCardById(Guid id)
        {
            var card = await _cardService.GetAllByIdAsync(id);
            var cardAppResult = card.ToResponseModel();

            return cardAppResult;
        }

        public async Task<List<GetCardsResponse>> FilterCardsAsync(Guid? id, int? month, int? year)
        {
            var card = await _cardService.FilterCardsAsync(id, month, year);
            var cardAppResult = card.Select(x => x.ToResponseModel()).ToList();


            var response = new List<GetCardsResponse>();
            foreach (var cardModel in cardAppResult)
            {
                foreach (var debt in cardModel.Debts)
                {
                    debt.Installments = debt.Installments.OrderBy(x => x.InstallmentNumber).ToList();
                }
                response.Add(cardModel);
            }            
            return response;
        }
    }
}
