using System;
using System.Collections.Generic;
using static MicroServices.WebDebts.Application.Models.EnumAppModel;

namespace MicroServices.WebDebts.Application.Models.DebtModels
{
    public class CreateDebtAppModel
    {
        public string Name { get; set; }
        public List<decimal> Values { get; set; }
        public decimal Value { get; set; }
        public DateTime Date { get; set; }
        public int NumberOfInstallments { get; set; }
        public DebtInstallmentTypeApp DebtInstallmentType { get; set; }
        public Guid CategoryId { get; set; }
        public bool IsGoal { get; set; } = false;
        public Guid? ResponsiblePartyId { get; set; }

    }
}
