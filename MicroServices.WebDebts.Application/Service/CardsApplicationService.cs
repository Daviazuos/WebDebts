using MicroServices.WebDebts.Application.Models;
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
        Task<Guid> CreateCard(CardAppModel cardAppModel);
        Task<Guid> AddValuesCard(DebtsAppModel debtsAppModel, string CardName);
        Task<GetCardByIdResponse> GetCardById(Guid id);
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

        public async Task<Guid> AddValuesCard(DebtsAppModel debtsAppModel, string CardName)
        {
            var debt = debtsAppModel.ToEntity();
            var debtCard = await _cardService.LinkCard(debt, CardName); 

            await _debtsService.CreateDebtAsync(debtCard, DebtType.Card);
            await _unitOfWork.CommitAsync();

            return debt.Id;
        }

        public async Task<Guid> CreateCard(CardAppModel cardAppModel)
        {
            var card = cardAppModel.ToEntity();
            var cardId = await _cardService.CreateCardAsync(card);
            await _unitOfWork.CommitAsync();

            return cardId;
        }

        public async Task<GetCardByIdResponse> GetCardById(Guid id)
        {
            var card = await _cardService.GetAllByIdAsync(id);

            var cardAppResult = card.ToResponseModel();

            return cardAppResult;
        }
    }
}
