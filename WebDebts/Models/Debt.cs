using System;
using System.Collections.Generic;

namespace MicroServices.WebDebts.Domain.Models
{
    public enum Status
    {
        Paid = 0,
        NotPaid = 1
    }

    public enum DebtStatus
    {
        Open = 0,
        Closed = 1
    }

    public enum DebtType
    {
        Installment = 0,
        Fixed = 1,
        Simple = 2
    }

    public class Debt
    {
        public string Name { get; set; }
        public decimal Value { get; set; }
        public DebtType DebtType { get; set; }
        public DebtStatus Status { get; set; }
        public IEnumerable<Installments> Installments { get; set; }

        // Adicionar AUTOMAPPER
        public static Debt Create(string name, decimal value, DebtType debtType, DebtStatus debtStatus, IEnumerable<Installments> installments)
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
        public DateTime PaymentDate { get; set; }
        public int InstallmentNumber { get; set; }
        public decimal Value { get; set; }
        public Status Status { get; set; }
    }
}
