using AutoMapper;
using MicroServices.WebDebts.Application.Models.DebtModels;
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

        public static GetCardsResponse ToResponseModel(this Card model)
        {
            return Mapper.Map<GetCardsResponse>(model);
        }

        public static CreateDebtAppModel ToCreateModel(this Debt entity)
        {
            return Mapper.Map<CreateDebtAppModel>(entity);
        }

        public static Debt ToCreateModel(this CreateDebtAppModel entity)
        {
            return Mapper.Map<Debt>(entity);
        }
    }
}
