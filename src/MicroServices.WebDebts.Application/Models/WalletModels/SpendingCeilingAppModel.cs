using MicroServices.WebDebts.Domain.Models.Enum;
using System;

namespace MicroServices.WebDebts.Application.Models
{
    public class SpendingCeilingAppModel
    {
        public decimal Value { get; set; }
        public DateTime Date { get; set; }
        public Guid CategoryId { get; set; }
    }
}
