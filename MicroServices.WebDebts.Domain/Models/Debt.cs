using MicroServices.WebDebts.Domain.Models.Enum;
using System;
using System.Collections.Generic;

namespace MicroServices.WebDebts.Domain.Models
{
    public class Debt : BaseDebt
    {
        public string Name { get; set; }
        public decimal Value { get; set; }
        public DebtType DebtType { get; set; }
        public DebtStatus Status { get; set; }
        public List<Installments> Installments { get; set; }

        // Adicionar AUTOMAPPER
        public static Debt Create(string name, decimal value, DebtType debtType, DebtStatus debtStatus, List<Installments> installments)
        {
            return new Debt
            {
                Name = name,
                Value = value,
                DebtType = debtType,
                Status = debtStatus,
                Installments = installments
            };
        }
    }

    public class Installments
    {
        public DateTime Date { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int InstallmentNumber { get; set; }
        public decimal Value { get; set; }
        public Status Status { get; set; }
    }
}
