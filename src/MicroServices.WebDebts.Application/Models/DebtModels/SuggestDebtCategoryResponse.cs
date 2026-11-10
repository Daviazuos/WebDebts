using System;

namespace MicroServices.WebDebts.Application.Models.DebtModels
{
    public class SuggestDebtCategoryResponse
    {
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public double Confidence { get; set; }
    }
}
