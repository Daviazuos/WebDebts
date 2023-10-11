using AutoMapper;
using MicroServices.WebDebts.Domain.Models;

namespace MicroServices.WebDebts.Application.Models.Mappers
{
    public static class SpendingCeilingMapper
    {
        static SpendingCeilingMapper()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<SpendingCeilingMapperProfile>())
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }

        public static SpendingCeilingAppModel ToModel(this SpendingCeiling entity)
        {
            return Mapper.Map<SpendingCeilingAppModel>(entity);
        }

        public static SpendingCeiling ToEntity(this SpendingCeilingAppModel model)
        {
            return Mapper.Map<SpendingCeiling>(model);
        }
    }
}
