using MicroServices.WebDebts.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroServices.WebDebts.Domain.Interfaces.Repository
{
    public interface IResponsiblePartyRepository : IBaseRepository<ResponsibleParty>
    {
        Task<List<ResponsibleParty>> GetByUserId(Guid userId);
    }
}
