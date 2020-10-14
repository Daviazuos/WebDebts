using MicroServices.WebDebts.Application.Services;
using MicroServices.WebDebts.Domain.Interfaces.Repository;
using MicroServices.WebDebts.Domain.Services;
using MicroServices.WebDebts.Infrastructure.Database.Postgres;
using MicroServices.WebDebts.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace MicroServices.WebDebts.DependencyInjection
{
    public class ApplicationStartup
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(options => options.UseNpgsql(configuration.GetConnectionString("DebtDB"), b => b.MigrationsAssembly("MicroServices.WebDebts.Infrastructure")));
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            RegisterApplication(services);
            RegisterDomain(services);
            RegisterInfrastructure(services);
        }

        public static void RegisterApplication(IServiceCollection services)
        {
            services.AddScoped<IDebtsApplicationService, DebtsApplicationService>();
        }

        public static void RegisterDomain(IServiceCollection services)
        {
            services.AddScoped<IDebtsService, DebtsService>();
        }

        public static void RegisterInfrastructure(IServiceCollection services)
        {
            services.AddScoped<IDebtRepository, DebtRepository>();
        }
    }
}