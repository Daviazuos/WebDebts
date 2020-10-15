using MicroServices.WebDebts.Domain.Models;
using MicroServices.WebDebts.Domain.Models.Enum;
using System.Collections.Generic;

namespace MicroServices.WebDebts.Application.Models
{
    public class GetDebtByIdResponse
    {
        public string Name { get; set; }
        public decimal Value { get; set; }
        public int NumberOfInstallments { get; set; }
        public DebtInstallmentType DebtType { get; set; }
        public List<Installments> Installments { get; set; }
    }
}
