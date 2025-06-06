﻿using AutoMapper;
using MicroServices.WebDebts.Domain.Models;
using System.Linq;

namespace MicroServices.WebDebts.Application.Models.Mappers
{
    public class WalletMapperProfile : Profile
    {
        public WalletMapperProfile()
        {
            this.CreateMap<Wallet, WalletAppModel>()
                .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
                .ReverseMap();

            this.CreateMap<Wallet, CreateWalletAppModel>()
                .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
                .ReverseMap();

            this.CreateMap<Wallet, GetWalletByIdResponse>()
                .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
                .ReverseMap();

            this.CreateMap<WalletInstallments, WalletInstallmentAppModel>()
                .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
                .ReverseMap();
        }
    }
}
