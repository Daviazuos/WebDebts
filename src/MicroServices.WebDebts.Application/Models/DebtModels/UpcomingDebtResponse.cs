using System;

namespace MicroServices.WebDebts.Application.Models
{
    public class UpcomingDebtResponse
    {
        public Guid Id { get; set; }
        public string Date { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string LastCharge { get; set; }
        public Decimal Price { get; set; }
        public string Logo { get; set; }
        public bool IsOverdue { get; set; }
    }
}
