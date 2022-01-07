using AutoMapper;
using MicroServices.WebDebts.Domain.Models;

namespace MicroServices.WebDebts.Application.Models.Mappers
{
    public class WalletMapperProfile : Profile
    {
        public WalletMapperProfile()
        {
            this.CreateMap<Wallet, WalletAppModel>()
                .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
                .ReverseMap();

            this.CreateMap<Wallet, GetWalletByIdResponse>()
                .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
                .ReverseMap();
        }
    }
}
