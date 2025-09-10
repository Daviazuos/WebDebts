using MicroServices.WebDebts.Application.Models.PlannerModels;
using MicroServices.WebDebts.Domain.Interfaces.Repository;
using MicroServices.WebDebts.Domain.Models;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using MicroServices.WebDebts.Application.Models.Mappers;

namespace MicroServices.WebDebts.Application.Service
{
    public class PlannerService : IPlannerService
    {
        private readonly IPlannerRepository _plannerRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserRepository _userRepository;


        public PlannerService(IPlannerRepository plannerRepository, ICategoryRepository categoryRepository, IUserRepository userRepository)
        {
            _plannerRepository = plannerRepository;
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
        }

        public async Task<PlannerResponse> AddPlannerAsync(CreatePlannerRequest request, Guid userId)
        {
            var plannerFrequencies = request.Blocks.Select((f, i) => new PlannerFrequency
                {
                    FrequencyNumber = i,
                    Start = f.StartDate,
                    End = f.EndDate,
                    PlannerCategories = new List<PlannerCategories>()
                })
                .ToList();

            var user = await _userRepository.FindByIdAsync(userId);


            var planner = new Planner
            {
                Id = Guid.NewGuid(),
                Frequency = request.Frequency,
                Month = request.Month,
                Year = request.Year,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                PlannerFrequencies = plannerFrequencies,
                User = user,
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

        public async Task<PlannerFrequencyResponse> AddPlannerCategoriesAsync(Guid plannerFrequencyId, AddPlannerCategoriesRequest request)
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

            return plannerFrequency.ToFrequencyResponse();


        }

        public async Task<List<PlannerResponse>> GetPlannersByUserAndMonthAsync(Guid userId, int month, int year)
        {
            var planners = await _plannerRepository.GetByUserAndMonthAsync(userId, month, year);

            return planners.Select(planner => new PlannerResponse
            {
                Id = planner.Id,
                Frequency = (Frequency)planner.Frequency,
                Month = planner.Month,
                Year = planner.Year,
                PlannerFrequencies = planner.PlannerFrequencies?.Select(f => new PlannerFrequencyResponse
                {
                    Id = f.Id,
                    FrequencyNumber = f.FrequencyNumber,
                    Start = f.Start,
                    End = f.End,
                    PlannerCategories = f.PlannerCategories?.Select(c => new PlannerCategoryResponse
                    {
                        Id = c.Id,
                        DebtCategoryId = c.DebtCategory?.Id ?? Guid.Empty,
                        DebtCategoryName = c.DebtCategory?.Name,
                        BudgetedValue = c.BudgetedValue
                    }).ToList()
                }).OrderBy(x => x.FrequencyNumber).ToList()
            }).ToList();
        }

        public async Task<PlannerCategoryResponse> UpdatePlannerCategoryBudgetAsync(Guid plannerCategoryId, decimal budgetedValue)
        {
            // Supondo que exista um método para buscar a PlannerCategory pelo ID
            var plannerCategory = await _plannerRepository.FindPlannerCategoryByIdAsync(plannerCategoryId);
            if (plannerCategory == null)
                throw new Exception("PlannerCategory not found");

            plannerCategory.BudgetedValue = budgetedValue;
            await _plannerRepository.UpdatePlannerCategoryAsync(plannerCategory);

            return new PlannerCategoryResponse
            {
                Id = plannerCategory.Id,
                DebtCategoryId = plannerCategory.DebtCategory?.Id ?? Guid.Empty,
                DebtCategoryName = plannerCategory.DebtCategory?.Name,
                BudgetedValue = plannerCategory.BudgetedValue
            };
        }

        public async Task DeletePlannerCategoryAsync(Guid plannerCategoryId)
        {
            // Supondo que já exista o método no repositório
            await _plannerRepository.DeletePlannerCategoryAsync(plannerCategoryId);
        }

        public async Task<PlannerFrequencyResponse> UpdatePlannerFrequencyDatesAsync(Guid plannerFrequencyId, DateTime start, DateTime end)
        {
            var plannerFrequency = await _plannerRepository.FindPlannerFrequencyByIdAsync(plannerFrequencyId);
            if (plannerFrequency == null)
                throw new Exception("PlannerFrequency not found");

            plannerFrequency.Start = start;
            plannerFrequency.End = end;
            await _plannerRepository.UpdatePlannerFrequencyAsync(plannerFrequency);

            return new PlannerFrequencyResponse
            {
                Id = plannerFrequency.Id,
                FrequencyNumber = plannerFrequency.FrequencyNumber,
                Start = plannerFrequency.Start,
                End = plannerFrequency.End,
                PlannerCategories = plannerFrequency.PlannerCategories?.Select(c => new PlannerCategoryResponse
                {
                    Id = c.Id,
                    DebtCategoryId = c.DebtCategory?.Id ?? Guid.Empty,
                    DebtCategoryName = c.DebtCategory?.Name,
                    BudgetedValue = c.BudgetedValue
                }).ToList()
            };
        }

        public async Task DeletePlannerFrequencyAsync(Guid plannerFrequencyId)
        {
            await _plannerRepository.DeletePlannerFrequencyAsync(plannerFrequencyId);
        }
    }
}