using AutoMapper;
using MicroServices.WebDebts.Application.Models.DebtModels;
using MicroServices.WebDebts.Domain.Models;

namespace MicroServices.WebDebts.Application.Models.Mappers
{
    public class CardMapperProfile : Profile
    {
        public CardMapperProfile()
        {
            this.CreateMap<Card, CardAppModel>()
                .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
                .ReverseMap();

            this.CreateMap<Card, GetCardsResponse>()
                .ForMember(x => x.Debts, opts => opts.MapFrom(x => x.DebtValues))
                .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
                .ReverseMap();

            this.CreateMap<Installments, InstallmentsAppModel>()
                .ReverseMap();

            this.CreateMap<Debt, DebtsAppModel>()
                .ForMember(x => x.Name, opts => opts.MapFrom(x => x.Name))
                .ForMember(x => x.Value, opts => opts.MapFrom(x => x.Value))
                .ForMember(x => x.NumberOfInstallments, opts => opts.MapFrom(x => x.NumberOfInstallments))
                .ForMember(x => x.DebtInstallmentType, opts => opts.MapFrom(x => x.DebtInstallmentType))
                .ForMember(x => x.Installments, opts => opts.MapFrom(x => x.Installments))
                .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
                .ReverseMap();

            this.CreateMap<Debt, CreateDebtAppModel>()
                .ForMember(x => x.Name, opts => opts.MapFrom(x => x.Name))
                .ForMember(x => x.Value, opts => opts.MapFrom(x => x.Value))
                .ForMember(x => x.NumberOfInstallments, opts => opts.MapFrom(x => x.NumberOfInstallments))
                .ForMember(x => x.DebtInstallmentType, opts => opts.MapFrom(x => x.DebtInstallmentType))
                .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
                .ReverseMap();
        }
    }
}
