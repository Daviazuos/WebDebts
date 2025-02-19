using System;

namespace MicroServices.WebDebts.Application.Models
{
    public class GetCardsRequest
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public Guid? Id { get; set; }
    }
}
