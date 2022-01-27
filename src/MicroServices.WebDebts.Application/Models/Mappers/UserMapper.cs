using AutoMapper;
using MicroServices.WebDebts.Domain.Models;


namespace MicroServices.WebDebts.Application.Models.Mappers
{
    public static class UserMapper
    {
        static UserMapper()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<UserProfile>())
                .CreateMapper();
        }

        internal static IMapper Mapper {  get; }

        public static UserAppModel ToModel(this User entity)
        {
            return Mapper.Map<UserAppModel>(entity);
        }

        public static User ToModel(this UserAppModel entity)
        {
            return Mapper.Map<User>(entity);
        }
    }
}
