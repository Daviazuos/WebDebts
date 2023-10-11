using System;

namespace MicroServices.WebDebts.Domain.Models
{
    public class Goal : ModelBase
    {
        public string Name { get; set; }
        public decimal TotalValue { get; set; }
        public DateTime InitialDate { get; set; }
        public int Years { get; set; }
        public string ImageUrl { get; set; }
        public Debt Debt { get; set; }
        public User User { get; set; }
    }
}
