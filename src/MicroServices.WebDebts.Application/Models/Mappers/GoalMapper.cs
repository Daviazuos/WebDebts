using AutoMapper;
using MicroServices.WebDebts.Domain.Models;

namespace MicroServices.WebDebts.Application.Models.Mappers
{
    public static class GoalMapper
    {
        static GoalMapper()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<GoalMapperProfile>())
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }

        public static GoalAppModel ToModel(this Goal entity)
        {
            return Mapper.Map<GoalAppModel>(entity);
        }

        public static Goal ToEntity(this GoalAppModel model)
        {
            return Mapper.Map<Goal>(model);
        }
    }
}
