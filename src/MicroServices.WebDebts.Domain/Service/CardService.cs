using MicroServices.WebDebts.Domain.Interfaces.Repository;
using MicroServices.WebDebts.Domain.Models;
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
        Task<List<Card>> FilterCardsAsync(Guid? id, int? month, int? year, Guid userId);
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

        public async Task<List<Card>> FilterCardsAsync(Guid? id, int? month, int? year, Guid userId)
        {
            var cards = await _cardRepository.FindCardValuesByIdAsync(id, userId, month, year);

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
