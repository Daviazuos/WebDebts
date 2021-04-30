using MicroServices.WebDebts.Domain.Models.Enum;
using System;

namespace MicroServices.WebDebts.Application.Models
{
    public class PutInstallmentsRequest
    {
        public Guid Id { get; set; }
        public Status InstallmentsStatus { get; set; }
    }
}
