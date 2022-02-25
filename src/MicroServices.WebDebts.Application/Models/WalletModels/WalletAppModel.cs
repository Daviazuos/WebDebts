using MicroServices.WebDebts.Domain.Models.Enum;
using System;

namespace MicroServices.WebDebts.Application.Models
{
    public class WalletAppModel
    {
        public string Name { get; set; }
        public decimal Value { get; set; }
        public DateTime InitialDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public WalletStatus WalletStatus { get; set; }
    }
}
