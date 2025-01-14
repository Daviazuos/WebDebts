using MicroServices.WebDebts.Domain.Models;
using MicroServices.WebDebts.Domain.Models.Commom;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroServices.WebDebts.Domain.Interfaces.Repository
{
    public interface ICardRepository : IBaseRepository<Card>
    {
        Task<Card> GetCardByName(string cardName, Guid userId);
        Task<Card> GetCardById(Guid id);
        Task<PaginatedList<Card>> FindCardValuesByIdAsync(int pageNumber, int pageSize, Guid? id, Guid userId, int? month, int? year, bool withNoDebts);
        Task<List<Debt>> FilterCardsAsync(int? month, int? year);
    }
}
