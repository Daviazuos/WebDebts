using System;

namespace MicroServices.WebDebts.Application.Models
{
    public class FilterInstallmentsRequest
    {
        public Guid? DebtId { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
    }
}
