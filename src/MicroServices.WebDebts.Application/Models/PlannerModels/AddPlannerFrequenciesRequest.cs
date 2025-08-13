using System;
using System.Collections.Generic;

namespace MicroServices.WebDebts.Application.Models.PlannerModels
{
    public class AddPlannerFrequenciesRequest
    {
        public List<PlannerFrequencyRequest> PlannerFrequencies { get; set; }
    }

    public class PlannerFrequencyRequest    
    {
        public int FrequencyNumber { get; set; }
        public List<PlannerCategoryRequest> PlannerCategories { get; set; }
    }

    public class PlannerCategoryRequest
    {
        public Guid DebtCategoryId { get; set; }
        public decimal BudgetedValue { get; set; }
    }
}