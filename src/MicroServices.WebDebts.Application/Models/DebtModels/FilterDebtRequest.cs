using System;
using static MicroServices.WebDebts.Application.Models.EnumAppModel;

namespace MicroServices.WebDebts.Application.Models
{
    public class FilterDebtRequest
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal? Value { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? FInishDate { get; set; }
        public DebtInstallmentTypeApp? DebtInstallmentType { get; set; }
        public DebtTypeApp? DebtType { get; set; }
        public bool? IsGoal { get; set; } = false;
    }
}
