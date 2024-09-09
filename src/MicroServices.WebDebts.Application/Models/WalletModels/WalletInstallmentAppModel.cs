using MicroServices.WebDebts.Domain.Models;
using MicroServices.WebDebts.Domain.Models.Enum;
using System;

namespace MicroServices.WebDebts.Application.Models
{
    public class WalletInstallmentAppModel
    {
        public DateTime? Date { get; set; }
        public int? InstallmentNumber { get; set; }
        public decimal? Value { get; set; }
        public bool? ReceivedStatus { get; set; }
        public string? Name { get; set; }
    }
}
