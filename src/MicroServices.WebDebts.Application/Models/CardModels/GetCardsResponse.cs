using System;
using System.Collections.Generic;

namespace MicroServices.WebDebts.Application.Models
{
    public class GetCardsResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int DueDate { get; set; }
        public int ClosureDate { get; set; }
        public List<DebtsAppModel> Debts { get; set; }
    }
}
