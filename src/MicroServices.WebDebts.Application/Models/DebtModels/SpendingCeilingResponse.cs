using MicroServices.WebDebts.Domain.Models.Enum;
using System;

namespace MicroServices.WebDebts.Application.Models.DebtModels
{
    public class SpendingCeilingResponse
    {
        public decimal PercentValue { get; set; }
        public DateTime Date { get; set; }
        public string CategoryName { get; set; }
        public decimal DebtValue { get; set; }
        public decimal SpendingCeilingValue { get; set; }
    }
}
