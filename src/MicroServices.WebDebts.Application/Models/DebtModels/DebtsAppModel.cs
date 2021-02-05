using System;
using static MicroServices.WebDebts.Application.Models.EnumAppModel;

namespace MicroServices.WebDebts.Application.Models
{
    public class DebtsAppModel
    {
        public string Name { get; set; }
        public decimal Value { get; set; }
        public DateTime Date { get; set; }
        public int NumberOfInstallments { get; set; }
        public DebtInstallmentTypeApp DebtInstallmentType { get; set; }
    }
}
