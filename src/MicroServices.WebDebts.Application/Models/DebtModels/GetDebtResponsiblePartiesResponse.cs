using MicroServices.WebDebts.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using static MicroServices.WebDebts.Application.Services.DebtsApplicationService;

namespace MicroServices.WebDebts.Application.Models
{
    public class GetDebtResponsiblePartiesResponse
    {
        public string Name { get; set; }
        public decimal DebtValue { get; set; }
        public decimal WalletValue { get; set; }
        public List<WalletAppModel> WalletAppModels { get; set; }
        public List<DebtsAppModel> DebtsAppModel { get; set; }
    }
}
