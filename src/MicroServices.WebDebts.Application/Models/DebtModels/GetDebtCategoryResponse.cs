using System;

namespace MicroServices.WebDebts.Application.Models
{
    public class GetDebtCategoryResponse
    {
        public string Name { get; set; }

        public decimal Value { get; set; }

        public int Month { get; set; }

        public decimal ValueTotal { get; set; }

        public decimal Percent { get; set; }
    }
}
