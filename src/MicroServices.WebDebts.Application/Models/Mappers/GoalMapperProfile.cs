using AutoMapper;
using MicroServices.WebDebts.Application.Models.DebtModels;
using MicroServices.WebDebts.Domain.Models;

namespace MicroServices.WebDebts.Application.Models.Mappers
{
    public class GoalMapperProfile : Profile
    {
        public GoalMapperProfile()
        {
            this.CreateMap<Goal, GoalAppModel>()
                .ReverseMap();
        }
    }
}
