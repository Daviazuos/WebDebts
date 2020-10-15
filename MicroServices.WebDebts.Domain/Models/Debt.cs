using MicroServices.WebDebts.Domain.Models.Enum;
using System;
using System.Collections.Generic;

namespace MicroServices.WebDebts.Domain.Models
{
    public class Debt : ModelBase
    {
        public string Name { get; set; }
        public decimal Value { get; set; }
        public DebtInstallmentType DebtInstallmentType { get; set; }
        public DebtType DebtType { get; set; }
        public DebtStatus Status { get; set; }
        public DateTime Date { get; set; }
        public int NumberOfInstallments { get; set; }
        public List<Installments> Installments { get; set; }
    }
}
