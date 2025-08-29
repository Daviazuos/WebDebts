using MicroServices.WebDebts.Application.Models.PlannerModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroServices.WebDebts.Application.Service
{
    public interface IPlannerService
    {
        Task<PlannerResponse> AddPlannerAsync(CreatePlannerRequest request);
        Task AddPlannerFrequenciesAsync(Guid plannerId, AddPlannerFrequenciesRequest request);
        Task<PlannerFrequencyResponse> AddPlannerCategoriesAsync(Guid plannerFrequencyId, AddPlannerCategoriesRequest request);
    }
}