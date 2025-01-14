using AutoMapper;
using MicroServices.WebDebts.Application.Models.ResponsiblePartyModels;
using MicroServices.WebDebts.Domain.Models;

namespace MicroServices.WebDebts.Application.Models.Mappers
{
    public class ResponsiblePartyMapperProfile : Profile
    {
        public ResponsiblePartyMapperProfile()
        {
            this.CreateMap<ResponsibleParty, ResponsiblePartyAppModel>()
                .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
                .ReverseMap();

            this.CreateMap<ResponsibleParty, ResponsiblePartyResponse>()
                .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
                .ReverseMap();
        }
    }
}
