using MicroServices.WebDebts.Domain.Models.Enum;
using System;
using System.Collections.Generic;

namespace MicroServices.WebDebts.Domain.Models
{
    public class DraftDebt : ModelBase
    {
        public string Name { get; set; }
        public decimal Value { get; set; }
        public DebtType DebtType { get; set; }
        public DateTime Date { get; set; }
        public DateTime BuyDate { get; set; }
        public Card Card { get; set; }
        public User User { get; set; }
        public string OriginalText { get; set; }
    }
}
