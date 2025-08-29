using AutoMapper;
using MicroServices.WebDebts.Application.Models.PlannerModels;
using MicroServices.WebDebts.Domain.Models;

namespace MicroServices.WebDebts.Application.Models.Mappers
{
    public static class PlannerMapper
    {
        static PlannerMapper()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<PlannerMapperProfile>())
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }

        public static PlannerResponse ToResponseModel(this Planner entity)
        {
            return Mapper.Map<PlannerResponse>(entity);
        }

        public static Planner ToEntity(this PlannerFrequencyRequest model)
        {
            return Mapper.Map<Planner>(model);
        }

        public static PlannerFrequency ToFrequencyEntity(this PlannerFrequencyResponse model)
        {
            return Mapper.Map<PlannerFrequency>(model);
        }

        public static PlannerFrequencyResponse ToFrequencyResponse(this PlannerFrequency model)
        {
            return Mapper.Map<PlannerFrequencyResponse>(model);
        }
    }

    public class PlannerMapperProfile : Profile
    {
        public PlannerMapperProfile()
        {
            CreateMap<Planner, PlannerResponse>()
                .ReverseMap();

            CreateMap<PlannerFrequencyRequest, Planner>()
                .ReverseMap();

            CreateMap<PlannerFrequencyResponse, PlannerFrequency>()
                .ReverseMap();

            CreateMap<PlannerCategoryResponse, PlannerCategories>()
                .ReverseMap();
        }
    }
}