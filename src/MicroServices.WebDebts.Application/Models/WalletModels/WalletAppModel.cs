using MicroServices.WebDebts.Application.Models.ResponsiblePartyModels;
using MicroServices.WebDebts.Domain.Models.Enum;
using System;
using System.Collections.Generic;

namespace MicroServices.WebDebts.Application.Models
{
    public class WalletAppModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
        public DateTime Date { get; set; }
        public WalletStatus WalletStatus { get; set; }
        public WalletInstallmentType WalletInstallmentType { get; set; }
        public int? NumberOfInstallments { get; set; }
        public ResponsiblePartyAppModel? ResponsiblePartyAppModel { get; set; }
        public List<WalletInstallmentAppModel> walletInstallments { get; set; }
    }
}
