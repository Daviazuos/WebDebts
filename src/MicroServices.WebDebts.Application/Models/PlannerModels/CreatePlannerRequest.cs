using MicroServices.WebDebts.Domain.Models;
using System;
using System.Collections.Generic;

namespace MicroServices.WebDebts.Application.Models.PlannerModels
{
    public class CreatePlannerRequest
    {
        public Frequency Frequency { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public List<BlocksFrequency> Blocks { get; set; }
    }

    public class BlocksFrequency
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}