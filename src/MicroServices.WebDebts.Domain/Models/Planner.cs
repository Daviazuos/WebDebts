using System;
using System.Collections.Generic;

namespace MicroServices.WebDebts.Domain.Models
{
    public class Planner : ModelBase
    {
        public Frequency Frequency { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public User User { get; set; }
        public List<PlannerFrequency> PlannerFrequencies { get; set; }
    }

    public class PlannerFrequency : ModelBase
    {
        public int FrequencyNumber { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public List<PlannerCategories> PlannerCategories { get; set; }
    }

    public class PlannerCategories : ModelBase
    {
        public DebtCategory DebtCategory { get; set; }
        public decimal BudgetedValue { get; set; }
    }

    public enum Frequency
    {
        Daily = 0,
        Weekly = 1,
        Biweekly = 2,
        Monthly = 3
    }
}