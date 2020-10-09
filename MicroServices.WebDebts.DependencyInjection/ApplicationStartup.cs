using MicroServices.WebDebts.Application.Services;
using MicroServices.WebDebts.Domain.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

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
        }

        public static void RegisterDomain(IServiceCollection services)
        {
            services.AddScoped<IDebtsService, DebtsService>();
        }
    }
}