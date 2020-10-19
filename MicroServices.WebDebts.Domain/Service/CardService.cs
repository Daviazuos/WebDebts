using MicroServices.WebDebts.Domain.Interfaces.Repository;
using MicroServices.WebDebts.Domain.Models;
using System;
using System.Threading.Tasks;

namespace MicroServices.WebDebts.Domain.Service
{
    public interface ICardService
    {
        Task<Guid> CreateCardAsync(Card card);

        Task<Debt> LinkCard(Debt debt, string CardName);
        Task<Card> GetAllByIdAsync(Guid id);
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
            return await _cardRepository.FindByIdAsync(id);
        }

        public async Task<Debt> LinkCard(Debt debt, string CardName)
        {
            var card = await _cardRepository.GetCardByName(CardName);

            debt.Card = card;

            return debt;
        }
    }
}
