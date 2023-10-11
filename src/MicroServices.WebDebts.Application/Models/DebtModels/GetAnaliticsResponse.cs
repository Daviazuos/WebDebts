using System;
using System.Collections.Generic;

namespace MicroServices.WebDebts.Application.Models
{
    public class GetAnaliticsResponse
    {
        public string NextCardName { get; set; }

        public string NextCardClosingDate { get; set; }

        public List<ValueByDay> Values { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

    }

    public class ValueByDay
    {
        public decimal Value { get; set; }

        public int Day { get; set; }
    }
}
