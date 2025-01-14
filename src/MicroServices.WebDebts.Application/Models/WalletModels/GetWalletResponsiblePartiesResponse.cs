using MicroServices.WebDebts.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using static MicroServices.WebDebts.Application.Services.DebtsApplicationService;

namespace MicroServices.WebDebts.Application.Models.WalletModels
{
    public class GetWalletResponsiblePartiesResponse
    {
        public string Name { get; set; }
        public decimal Value { get; set; }
        public List<WalletAppModel> WalletAppModels { get; set; }
    }
}
