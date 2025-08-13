using MicroServices.WebDebts.Domain.Models;
using System;

namespace MicroServices.WebDebts.Application.Models.PlannerModels
{
    public class CreatePlannerRequest
    {
        public Frequency Frequency { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }
}