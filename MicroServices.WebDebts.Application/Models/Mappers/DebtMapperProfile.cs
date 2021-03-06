﻿using AutoMapper;
using MicroServices.WebDebts.Application.Models.DebtModels;
using MicroServices.WebDebts.Domain.Models;

namespace MicroServices.WebDebts.Application.Models.Mappers
{
    public class DebtMapperProfile : Profile
    {
        public DebtMapperProfile()
        {
            this.CreateMap<Debt, DebtsAppModel>()
                .ForMember(x => x.Name, opts => opts.MapFrom(x => x.Name))
                .ForMember(x => x.Value, opts => opts.MapFrom(x => x.Value))
                .ForMember(x => x.NumberOfInstallments, opts => opts.MapFrom(x => x.NumberOfInstallments))
                .ForMember(x => x.DebtInstallmentType, opts => opts.MapFrom(x => x.DebtInstallmentType))
                .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
                .ReverseMap();

            this.CreateMap<Debt, GetDebtByIdResponse>()
                .ForMember(x => x.Name, opts => opts.MapFrom(x => x.Name))
                .ForMember(x => x.Value, opts => opts.MapFrom(x => x.Value))
                .ForMember(x => x.NumberOfInstallments, opts => opts.MapFrom(x => x.NumberOfInstallments))
                .ForMember(x => x.DebtInstallmentType, opts => opts.MapFrom(x => x.DebtInstallmentType))
                .ForMember(x => x.Installments, opts => opts.MapFrom(x =>x.Installments))
                .ForMember(x => x.DebtType, opts => opts.MapFrom(x =>x.DebtType))
                .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
                .ReverseMap();
            
            this.CreateMap<Installments, InstallmentsAppModel>()
                .ReverseMap();
        }
    }
}
