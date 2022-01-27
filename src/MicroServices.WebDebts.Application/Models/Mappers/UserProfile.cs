using AutoMapper;
using MicroServices.WebDebts.Domain.Models;

namespace MicroServices.WebDebts.Application.Models.Mappers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            this.CreateMap<User, UserProfile>()
                .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
                .ReverseMap()
;

        }
    }
}
