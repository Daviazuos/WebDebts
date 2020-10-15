using AutoMapper;
using MicroServices.WebDebts.Domain.Models;

namespace MicroServices.WebDebts.Application.Models.Mappers
{
    public static class CardMapper
    {
        static CardMapper()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<CardMapperProfile>())
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }

        public static CardAppModel ToModel(this Card entity)
        {
            return Mapper.Map<CardAppModel>(entity);
        }

        public static Card ToEntity(this CardAppModel model)
        {
            return Mapper.Map<Card>(model);
        }
    }
}
