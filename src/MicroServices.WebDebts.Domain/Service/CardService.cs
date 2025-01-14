using MicroServices.WebDebts.Domain.Interfaces.Repository;
using MicroServices.WebDebts.Domain.Models;
using MicroServices.WebDebts.Domain.Models.Commom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroServices.WebDebts.Domain.Service
{
    public interface ICardService
    {
        Task<Guid> CreateCardAsync(Card card);
        Task<Debt> LinkCard(Debt debt, Guid Id);
        Task<Card> GetAllByIdAsync(Guid id);
        Task<PaginatedList<Card>> FilterCardsAsync(int pageNumber, int pageSize, Guid? id, int? month, int? year, Guid userId, bool withNoDebts);
    }
    public class CardService : ICardService
    {
        private readonly ICardRepository _cardRepository;
        public CardService(ICardRepository cardRepository)
        {
            _cardRepository = cardRepository;
        }

        public async Task<Guid> CreateCardAsync(Card card)
        {
            card.Id = Guid.NewGuid();
            card.CreatedAt = DateTime.Now;
            
            await _cardRepository.AddAsync(card);
            
            return card.Id;
        }

        public async Task<Card> GetAllByIdAsync(Guid id)
        {
            return await _cardRepository.GetCardById(id);
        }

        public async Task<PaginatedList<Card>> FilterCardsAsync(int pageNumber, int pageSize, Guid? id, int? month, int? year, Guid userId, bool withNoDebts)
        {
            var cards = await _cardRepository.FindCardValuesByIdAsync(pageNumber, pageSize, id, userId, month, year, withNoDebts);

            return cards;
        }

        public async Task<Debt> LinkCard(Debt debt, Guid Id)
        {
            var card = await _cardRepository.FindByIdAsync(Id);

            debt.Card = card;

            return debt;
        }
    }
}
