using System;
using System.Collections.Generic;
using static MicroServices.WebDebts.Application.Models.EnumAppModel;

namespace MicroServices.WebDebts.Application.Models.DebtModels
{
    public class UpdateDebtAppModel
    {
        public string Name { get; set; }
        public decimal Value { get; set; }
        public DateTime Date { get; set; }
        public int NumberOfInstallments { get; set; }
        public DebtInstallmentTypeApp DebtInstallmentType { get; set; }
        public Guid CategoryId { get; set; }
        public bool IsGoal { get; set; } = false;
    }
}
