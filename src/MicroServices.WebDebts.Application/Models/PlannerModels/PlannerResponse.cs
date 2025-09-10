using MicroServices.WebDebts.Domain.Models;
using System;
using System.Collections.Generic;

namespace MicroServices.WebDebts.Application.Models.PlannerModels
{
    public class PlannerResponse
    {
        public Guid Id { get; set; }
        public Frequency Frequency { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public List<PlannerFrequencyResponse> PlannerFrequencies { get; set; }
    }

    public class PlannerFrequencyResponse
    {
        public Guid Id { get; set; }
        public int FrequencyNumber { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public List<PlannerCategoryResponse> PlannerCategories { get; set; }
    }

    public class PlannerCategoryResponse
    {
        public Guid Id { get; set; }
        public Guid DebtCategoryId { get; set; }
        public string DebtCategoryName { get; set; }
        public decimal BudgetedValue { get; set; }
    }
}