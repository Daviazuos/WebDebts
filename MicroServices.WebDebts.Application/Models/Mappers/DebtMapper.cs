using AutoMapper;
using MicroServices.WebDebts.Domain.Models;

namespace MicroServices.WebDebts.Application.Models.Mappers
{
    public static class DebtMapper
    {
        static DebtMapper()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<DebtMapperProfile>())
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }

        public static DebtsAppModel ToModel(this Debt entity)
        {
            return Mapper.Map<DebtsAppModel>(entity);
        }

        public static Debt ToEntity(this DebtsAppModel model)
        {
            return Mapper.Map<Debt>(model);
        }
    }
}
