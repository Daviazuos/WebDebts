using System;

namespace MicroServices.WebDebts.Domain.Models
{
    public class SpendingCeiling : ModelBase
    {
        public decimal Value { get; set; }
        public DateTime Date { get; set; }
        public User User { get; set; }
        public DebtCategory DebtCategory { get; set; }
    }

}
