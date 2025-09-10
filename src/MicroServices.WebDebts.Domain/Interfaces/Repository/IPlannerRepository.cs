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
        Task<List<Planner>> GetByUserAndMonthAsync(Guid userId, int month, int year);
        Task<PlannerCategories> FindPlannerCategoryByIdAsync(Guid plannerCategoryId);
        Task UpdatePlannerCategoryAsync(PlannerCategories plannerCategory);
        Task DeletePlannerCategoryAsync(Guid plannerCategoryId);
        Task DeletePlannerFrequencyAsync(Guid plannerFrequencyId);
    }
}