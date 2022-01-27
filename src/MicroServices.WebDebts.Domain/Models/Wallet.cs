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
        public DateTime StartAt {  get; set; }
        public DateTime? FinishAt {  get; set; }
        public List<WalletMonthController> WalletMonthControllers { get; set; }
        public User User { get; set; }

    }

    public class WalletMonthController : ModelBase
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal UpdatedValue { get; set; }
        public Wallet Wallet { get; set; }
        public User User { get; set; }

    }
}
