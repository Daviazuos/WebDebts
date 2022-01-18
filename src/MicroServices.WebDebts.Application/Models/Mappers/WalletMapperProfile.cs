using AutoMapper;
using MicroServices.WebDebts.Domain.Models;
using System.Linq;

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
                .ForMember(x => x.UpdatedValue, opts => opts.MapFrom(x => x.WalletMonthControllers.FirstOrDefault().UpdatedValue))
                .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
                .ReverseMap();
        }
    }
}
