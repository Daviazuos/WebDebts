using System;

namespace MicroServices.WebDebts.Application.Models
{
    public class GetCardByIdResponse
    {
        public string Name { get; set; }
        public int DueDate { get; set; }
        public int ClosureDate { get; set; }
    }
}
