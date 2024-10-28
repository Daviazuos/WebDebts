using MicroServices.WebDebts.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using static MicroServices.WebDebts.Application.Services.DebtsApplicationService;

namespace MicroServices.WebDebts.Application.Models
{
    public class GetDebtCategoryResponse
    {
        public string Name { get; set; }

        public decimal Value { get; set; }

        public int Month { get; set; }

        public decimal ValueTotal { get; set; }

        public decimal Percent { get; set; }
        public List<InstallmentDto> InstallmentsPerCategory { get; set; }
    }
}
