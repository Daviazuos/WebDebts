using MicroServices.WebDebts.Domain.Models.Enum;
using System;

namespace MicroServices.WebDebts.Domain.Models
{
    public class Installments : ModelBase
    {
        public DateTime Date { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int InstallmentNumber { get; set; }
        public decimal Value { get; set; }
        public Status Status { get; set; }
        public Debt Debt { get; set; }
        public Guid? WalletMonthControllerId { get; set; }
        public User User { get; set; }
    }
}
