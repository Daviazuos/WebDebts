using MicroServices.WebDebts.Application.Services;
using MicroServices.WebDebts.Domain.Interfaces.Repository;
using MicroServices.WebDebts.Domain.Services;
using MicroServices.WebDebts.;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MicroServices.WebDebts.DependencyInjection
{
    public class ApplicationStartup
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            RegisterApplication(services);
            RegisterDomain(services);
        }

        public static void RegisterApplication(IServiceCollection services)
        {
            services.AddScoped<IDebtsApplicationService, DebtsApplicationService>();
            services.AddScoped<IDebtRepository, BaseRepository>();
        }

        public static void RegisterDomain(IServiceCollection services)
        {
            services.AddScoped<IDebtsService, DebtsService>();
        }
    }
}