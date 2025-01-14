using AutoMapper;
using MicroServices.WebDebts.Application.Models.ResponsiblePartyModels;
using MicroServices.WebDebts.Domain.Models;

namespace MicroServices.WebDebts.Application.Models.Mappers
{
    public static class ResponsiblePartyMapper
    {
        static ResponsiblePartyMapper()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<ResponsiblePartyMapperProfile>())
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }

        public static ResponsiblePartyAppModel ToModel(this ResponsibleParty entity)
        {
            return Mapper.Map<ResponsiblePartyAppModel>(entity);
        }

        public static ResponsibleParty ToEntity(this ResponsiblePartyAppModel model)
        {
            return Mapper.Map<ResponsibleParty>(model);
        }

        public static ResponsiblePartyResponse ToResponseModel(this ResponsibleParty entity)
        {
            return Mapper.Map<ResponsiblePartyResponse>(entity);
        }
    }
}
