using System;

namespace MicroServices.WebDebts.Application.Models.DebtModels
{
    public class GoalResponse
    {
        public string Name { get; set; }
        public decimal TotalValue { get; set; }
        public DateTime InitialDate { get; set; }
        public int Years { get; set; }
        public string ImageUrl { get; set; }
    }
}
