using System;
using static MicroServices.WebDebts.Application.Models.EnumAppModel;

namespace MicroServices.WebDebts.Application.Models
{
    public class FilterInstallmentsResponse
    {
        public Guid Id { get; set; }
        public string? DebtName { get; set; }
        public DateTime Date { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int InstallmentNumber { get; set; }
        public int NumberOfInstallments { get; set; }
        public decimal Value { get; set; }
        public StatusApp Status { get; set; }
        public string? Category { get; set; }
        public string? ResponsiblePartyName { get; set; }
    }
}
