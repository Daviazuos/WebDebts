using MicroServices.WebDebts.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroServices.WebDebts.Domain.Interfaces.Repository
{
    public interface IGoalRepository : IBaseRepository<Goal>
    {
        Task<Goal> GetGoalById(Guid id);
        Task<List<Goal>> FilterGoalAsync(Guid? id, Guid userId);
    }
}
