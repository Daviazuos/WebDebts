using System;

namespace MicroServices.WebDebts.Domain.Models
{
    public class WalletInstallments : ModelBase
    {
        public DateTime Date { get; set; }
        public int InstallmentNumber { get; set; }
        public decimal Value { get; set; }
        public Wallet Wallet { get; set; }
        public User User { get; set; }
    }
}
