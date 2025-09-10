using MicroServices.WebDebts.Application.Models.PlannerModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroServices.WebDebts.Application.Service
{
    public interface IPlannerService
    {
        Task<PlannerResponse> AddPlannerAsync(CreatePlannerRequest request, Guid userId);
        Task AddPlannerFrequenciesAsync(Guid plannerId, AddPlannerFrequenciesRequest request);
        Task<PlannerFrequencyResponse> AddPlannerCategoriesAsync(Guid plannerFrequencyId, AddPlannerCategoriesRequest request);
        Task<List<PlannerResponse>> GetPlannersByUserAndMonthAsync(Guid userId, int month, int year);
        Task<PlannerCategoryResponse> UpdatePlannerCategoryBudgetAsync(Guid plannerCategoryId, decimal budgetedValue);
        Task DeletePlannerCategoryAsync(Guid plannerCategoryId);
        Task<PlannerFrequencyResponse> UpdatePlannerFrequencyDatesAsync(Guid plannerFrequencyId, DateTime start, DateTime end);
        Task DeletePlannerFrequencyAsync(Guid plannerFrequencyId);
    }
}