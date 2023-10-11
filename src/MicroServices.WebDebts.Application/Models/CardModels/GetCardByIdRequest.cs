using System;

namespace MicroServices.WebDebts.Application.Models
{
    public class GetCardByIdRequest
    {
        public Guid? Id { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
    }
}
