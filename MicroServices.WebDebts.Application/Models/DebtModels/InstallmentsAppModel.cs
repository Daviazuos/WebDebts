using System;
using static MicroServices.WebDebts.Application.Models.EnumAppModel;

namespace MicroServices.WebDebts.Application.Models.DebtModels
{
    public class InstallmentsAppModel
    {
        public DateTime Date { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int InstallmentNumber { get; set; }
        public decimal Value { get; set; }
        public StatusApp Status { get; set; }
    }
}
