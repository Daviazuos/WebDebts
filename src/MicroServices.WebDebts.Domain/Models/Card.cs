using System;
using System.Collections.Generic;

namespace MicroServices.WebDebts.Domain.Models
{
    public class Card : ModelBase
    {
        public string Name { get; set; }
        public int DueDate { get; set; }
        public int ClosureDate { get; set; }
        public string Color { get; set; }
        public List<Debt> DebtValues { get; set; }
        public User User { get; set; }
    }
}
