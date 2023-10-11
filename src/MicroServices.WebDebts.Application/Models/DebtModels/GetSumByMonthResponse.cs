
using System;

namespace MicroServices.WebDebts.Application.Models
{
    public class GetSumbyMonthResponse
    {
        public string Month { get; set; }
        public int Year { get; set; }
        public decimal DebtValue { get; set; }
        public decimal WalletValue { get; set; }
        public DateTime OriginalDate { get; set; }

    }
}
