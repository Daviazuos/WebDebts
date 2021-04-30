using AutoMapper;
using MicroServices.WebDebts.Application.Models.DebtModels;
using MicroServices.WebDebts.Domain.Models;

namespace MicroServices.WebDebts.Application.Models.Mappers
{
    public static class InstallmentsMapper
    {
        static InstallmentsMapper()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<InstallmentsMapperProfile>())
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }

        public static InstallmentsAppModel ToModel(this Installments entity)
        {
            return Mapper.Map<InstallmentsAppModel>(entity);
        }

        public static Installments ToEntity(this InstallmentsAppModel model)
        {
            return Mapper.Map<Installments>(model);
        }

        public static FilterInstallmentsResponse ToResponse(this Installments entity)
        {
            return Mapper.Map<FilterInstallmentsResponse>(entity);
        }
    }
}
