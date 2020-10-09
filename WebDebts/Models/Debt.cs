using MicroServices.WebDebts.Domain.Enum;
using System;
using System.Collections.Generic;

namespace MicroServices.WebDebts.Domain.Models
{
    public class Debt : BaseModel
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
}
