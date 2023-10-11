using System;
using static MicroServices.WebDebts.Application.Models.EnumAppModel;

namespace MicroServices.WebDebts.Application.Models
{
    public class FilterInstallmentsRequest
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public Guid? DebtId { get; set; }
        public Guid? CardId { get; set; }
        public DebtInstallmentTypeApp? DebtInstallmentType { get; set; }
        public StatusApp? StatusApp { get; set; }
        public DebtTypeApp? DebtType { get; set; }
        public bool? IsGoal { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
    }
}
