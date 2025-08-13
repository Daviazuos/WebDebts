using System;
using System.Collections.Generic;

namespace MicroServices.WebDebts.Application.Models.PlannerModels
{
    public class AddPlannerCategoriesRequest
    {
        public Guid DebtCategoryId { get; set; }
        public decimal BudgetedValue { get; set; }
    }
}