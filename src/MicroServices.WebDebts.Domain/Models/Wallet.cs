using MicroServices.WebDebts.Domain.Models.Enum;
using System;

namespace MicroServices.WebDebts.Domain.Models
{
    public class Wallet : ModelBase
    {
        public string Name { get; set; }
        public Guid? HistoryId { get; set; }
        public decimal Value { get; set; }
        public WalletStatus WalletStatus {  get; set; }
        public DateTime StartAt {  get; set; }
        public DateTime? FinishAt {  get; set; }
    }
}
