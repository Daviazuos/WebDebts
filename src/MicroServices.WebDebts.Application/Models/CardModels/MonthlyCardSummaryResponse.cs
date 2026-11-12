using System;

namespace MicroServices.WebDebts.Application.Models.CardModels
{
    public class MonthlyCardSummaryResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int DueDate { get; set; }
        public int ClosureDate { get; set; }
        public string Color { get; set; }
        public decimal TotalValue { get; set; }
        public bool IsPaid { get; set; }
    }
}
