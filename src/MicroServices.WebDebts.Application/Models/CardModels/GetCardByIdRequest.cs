using System;

namespace MicroServices.WebDebts.Application.Models
{
    public class GetCardByIdRequest
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public Guid? Id { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
        public bool WithNoDebts { get; set; }
    }
}
