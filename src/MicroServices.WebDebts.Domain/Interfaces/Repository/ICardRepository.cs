using MicroServices.WebDebts.Domain.Models;
using System;
using System.Threading.Tasks;

namespace MicroServices.WebDebts.Domain.Interfaces.Repository
{
    public interface ICardRepository : IBaseRepository<Card>
    {
        Task<Card> GetCardByName(string cardName);
        Task<Card> FindCardValuesByIdAsync(Guid id);
    }
}
