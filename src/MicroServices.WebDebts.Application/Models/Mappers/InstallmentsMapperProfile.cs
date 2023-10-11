using AutoMapper;
using MicroServices.WebDebts.Application.Models.DebtModels;
using MicroServices.WebDebts.Domain.Models;

namespace MicroServices.WebDebts.Application.Models.Mappers
{
    public class InstallmentsMapperProfile : Profile
    {
        public InstallmentsMapperProfile()
        {
            this.CreateMap<Installments, InstallmentsAppModel>()
                .ReverseMap();

            this.CreateMap<Installments, FilterInstallmentsResponse>()
                .ReverseMap();
        }
    }
}
