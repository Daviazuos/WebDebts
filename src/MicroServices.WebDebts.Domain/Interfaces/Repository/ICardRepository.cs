using MicroServices.WebDebts.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroServices.WebDebts.Domain.Interfaces.Repository
{
    public interface ICardRepository : IBaseRepository<Card>
    {
        Task<Card> GetCardByName(string cardName, Guid userId);
        Task<Card> GetCardById(Guid id);
        Task<List<Card>> FindCardValuesByIdAsync(Guid? id, Guid userId);
        Task<List<Debt>> FilterCardsAsync(int? month, int? year);
    }
}
