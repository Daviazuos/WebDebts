using MicroServices.WebDebts.Domain.Models.Enum;

namespace MicroServices.WebDebts.Application.Models
{
    public class WalletAppModel
    {
        public string Name { get; set; }
        public decimal Value { get; set; }
        public WalletStatus WalletStatus { get; set; }
    }
}
