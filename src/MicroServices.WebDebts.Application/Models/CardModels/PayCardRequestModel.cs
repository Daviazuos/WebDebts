using System;

namespace MicroServices.WebDebts.Application.Models
{
    public class PayCardResponseModel
    {
        public Guid Id { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime CorrespondingDate { get; set; }
        public Guid WalletId { get; set; }
    }
}
