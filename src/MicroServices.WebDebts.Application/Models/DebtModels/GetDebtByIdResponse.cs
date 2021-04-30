using MicroServices.WebDebts.Application.Models.DebtModels;
using System;
using System.Collections.Generic;
using static MicroServices.WebDebts.Application.Models.EnumAppModel;

namespace MicroServices.WebDebts.Application.Models
{
    public class GetDebtByIdResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
        public int NumberOfInstallments { get; set; }
        public DebtTypeApp DebtType { get; set; }
        public DebtInstallmentTypeApp DebtInstallmentType { get; set; }
        public List<InstallmentsAppModel> Installments { get; set; }
    }
}
