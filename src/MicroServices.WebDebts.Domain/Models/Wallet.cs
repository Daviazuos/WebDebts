using MicroServices.WebDebts.Domain.Models.Enum;
using System;
using System.Collections.Generic;

namespace MicroServices.WebDebts.Domain.Models
{
    public class Wallet : ModelBase
    {
        public string Name { get; set; }
        public Guid? HistoryId { get; set; }
        public decimal Value { get; set; }
        public WalletStatus WalletStatus {  get; set; }
        public WalletInstallmentType WalletInstallmentType { get; set; }
        public int? NumberOfInstallments { get; set; }
        public DateTime? StartAt {  get; set; }
        public DateTime Date {  get; set; }
        public DateTime? FinishAt {  get; set; }
        public List<WalletInstallments> WalletInstallments { get; set; }
        public User User { get; set; }
        public ResponsibleParty? ResponsibleParty { get; set; }
    }
}
