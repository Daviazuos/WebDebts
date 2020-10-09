using MicroServices.WebDebts.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroServices.WebDebts.Domain.Models
{
    public class Installments
    {
        public DateTime Date { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int InstallmentNumber { get; set; }
        public decimal Value { get; set; }
        public Status Status { get; set; }
    }
}
