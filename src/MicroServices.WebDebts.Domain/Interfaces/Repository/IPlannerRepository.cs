using MicroServices.WebDebts.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroServices.WebDebts.Domain.Interfaces.Repository
{
    public interface IPlannerRepository : IBaseRepository<Planner>
    {
        Task<Planner> AddAsync(Planner planner);
        Task<PlannerFrequency> FindPlannerFrequencyByIdAsync(Guid plannerFrequencyId);
        Task<List<Planner>> GetByUserIdAsync(Guid userId);
        Task UpdatePlannerFrequencyAsync(PlannerFrequency plannerFrequency);
    }
}