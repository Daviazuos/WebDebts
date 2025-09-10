using System;

namespace MicroServices.WebDebts.Application.Models.PlannerModels
{
    public class UpdatePlannerFrequencyDatesRequest
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}