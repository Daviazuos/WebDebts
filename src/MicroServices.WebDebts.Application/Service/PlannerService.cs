using MicroServices.WebDebts.Application.Models.PlannerModels;
using MicroServices.WebDebts.Domain.Interfaces.Repository;
using MicroServices.WebDebts.Domain.Models;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace MicroServices.WebDebts.Application.Service
{
    public class PlannerService : IPlannerService
    {
        private readonly IPlannerRepository _plannerRepository;
        private readonly ICategoryRepository _categoryRepository;

        public PlannerService(IPlannerRepository plannerRepository, ICategoryRepository categoryRepository)
        {
            _plannerRepository = plannerRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<PlannerResponse> AddPlannerAsync(CreatePlannerRequest request)
        {
            int frequencyCount = request.Frequency switch
            {
                Frequency.Daily => 30,
                Frequency.Weekly => 4,
                Frequency.Biweekly => 2,
                Frequency.Monthly => 1,
                _ => 1
            };

            var plannerFrequencies = Enumerable.Range(1, frequencyCount)
                .Select(i => new PlannerFrequency
                {
                    FrequencyNumber = i,
                    PlannerCategories = new List<PlannerCategories>()
                })
                .ToList();

            var planner = new Planner
            {
                Id = Guid.NewGuid(),
                Frequency = request.Frequency,
                Month = request.Month,
                Year = request.Year,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                PlannerFrequencies = plannerFrequencies
            };

            await _plannerRepository.AddAsync(planner);

            return new PlannerResponse
            {
                Id = planner.Id,
                Frequency = request.Frequency,
                Month = request.Month,
                Year = request.Year,
                PlannerFrequencies = plannerFrequencies
                    .Select(f => new PlannerFrequencyResponse
                    {
                        Id = f.Id,
                        FrequencyNumber = f.FrequencyNumber,
                        PlannerCategories = new List<PlannerCategoryResponse>()
                    })
                    .ToList()
            };
        }

        public async Task AddPlannerFrequenciesAsync(Guid plannerId, AddPlannerFrequenciesRequest request)
        {
            var planner = await _plannerRepository.FindByIdAsync(plannerId);
            if (planner == null)
                throw new Exception("Planner not found");

            planner.PlannerFrequencies = new List<PlannerFrequency>();

            foreach (var f in request.PlannerFrequencies)
            {
                var categoriesTasks = f.PlannerCategories.Select(async c => new PlannerCategories
                {
                    DebtCategory = await _categoryRepository.FindByIdAsync(c.DebtCategoryId),
                    BudgetedValue = c.BudgetedValue
                });

                var categories = await Task.WhenAll(categoriesTasks);

                planner.PlannerFrequencies.Add(new PlannerFrequency
                {
                    FrequencyNumber = f.FrequencyNumber,
                    PlannerCategories = categories.ToList()
                });
            }

            planner.UpdatedAt = DateTime.UtcNow;
            await _plannerRepository.UpdateAsync(planner);
        }

        public async Task AddPlannerCategoriesAsync(Guid plannerFrequencyId, AddPlannerCategoriesRequest request)
        {
            var plannerFrequency = await _plannerRepository.FindPlannerFrequencyByIdAsync(plannerFrequencyId);
            if (plannerFrequency == null)
                throw new Exception("PlannerFrequency not found");
            
            var debtCategory = await _categoryRepository.FindByIdAsync(request.DebtCategoryId);
            var category = new PlannerCategories
            {
                DebtCategory = debtCategory,
                BudgetedValue = request.BudgetedValue
            };
            

            plannerFrequency.PlannerCategories.Add(category);
            await _plannerRepository.UpdatePlannerFrequencyAsync(plannerFrequency);


        }
    }
}