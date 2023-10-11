﻿using AutoMapper;
using MicroServices.WebDebts.Application.Models.DebtModels;
using MicroServices.WebDebts.Domain.Models;

namespace MicroServices.WebDebts.Application.Models.Mappers
{
    public class SpendingCeilingMapperProfile : Profile
    {
        public SpendingCeilingMapperProfile()
        {
            this.CreateMap<SpendingCeiling, SpendingCeilingAppModel>()
                .ReverseMap();
        }
    }
}
