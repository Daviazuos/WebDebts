using MicroServices.WebDebts.Domain.Models.Enum;
using System.Collections.Generic;

namespace MicroServices.WebDebts.Domain.Models
{
    public class Debt : BaseDebt
    {
        public string Name { get; set; }
        public decimal Value { get; set; }
        public DebtType DebtType { get; set; }
        public DebtStatus Status { get; set; }
        public int NumberOfInstallments { get; set; }
        public List<Installments> Installments { get; set; }
    }
}
