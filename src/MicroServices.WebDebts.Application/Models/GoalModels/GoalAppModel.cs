using System;

namespace MicroServices.WebDebts.Application.Models
{
    public class GoalAppModel
    {
        public string Name { get; set; }
        public decimal TotalValue { get; set; }
        public DateTime InitialDate { get; set; }
        public int Years { get; set; }
        public string ImageUrl { get; set; }
        public DebtsAppModel Debt { get; set; }
    }
}
