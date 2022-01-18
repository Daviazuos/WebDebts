using System;
using static MicroServices.WebDebts.Application.Models.EnumAppModel;

namespace MicroServices.WebDebts.Application.Models
{
    public class FilterInstallmentsRequest
    {
        public int PageNumber { get; set; }
        public Guid? DebtId { get; set; }
        public DebtInstallmentTypeApp? DebtInstallmentType { get; set; }
        public StatusApp? StatusApp { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
    }
}
