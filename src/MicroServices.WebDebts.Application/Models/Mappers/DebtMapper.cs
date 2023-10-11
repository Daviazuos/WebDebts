using AutoMapper;
using MicroServices.WebDebts.Application.Models.DebtModels;
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

        public static GetDebtByIdResponse ToResponseModel(this Debt entity)
        {
            return Mapper.Map<GetDebtByIdResponse>(entity);
        }

        public static CreateDebtAppModel ToCreateModel(this Debt entity)
        {
            return Mapper.Map<CreateDebtAppModel>(entity);
        }

        public static CreateCategoryRequest ToModel(this DebtCategory entity)
        {
            return Mapper.Map<CreateCategoryRequest>(entity);
        }

        public static DebtCategory ToEntity(this CreateCategoryRequest model)
        {
            return Mapper.Map<DebtCategory>(model);
        }
    }
}
