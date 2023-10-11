﻿using AutoMapper;
using MicroServices.WebDebts.Domain.Models;

namespace MicroServices.WebDebts.Application.Models.Mappers
{
    public static class WalletMapper
    {
        static WalletMapper()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<WalletMapperProfile>())
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }

        public static WalletAppModel ToModel(this Wallet entity)
        {
            return Mapper.Map<WalletAppModel>(entity);
        }

        public static Wallet ToEntity(this WalletAppModel model)
        {
            return Mapper.Map<Wallet>(model);
        }

        public static GetWalletByIdResponse ToResponseModel(this Wallet entity)
        {
            return Mapper.Map<GetWalletByIdResponse>(entity);
        }
    }
}
