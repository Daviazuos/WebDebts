using MicroServices.WebDebts.Domain.Models.Enum;
using System;

namespace MicroServices.WebDebts.Application.Models
{
    public class GetWalletByIdResponse
    {
        public Guid Id {  get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
        public DateTime Date { get; set; }
        public WalletStatus WalletStatus { get; set; }
        public WalletInstallmentType WalletInstallmentType { get; set; }
        public int? NumberOfInstallments { get; set; }
        public int InstallmentNumber { get; set; }
    }
}
