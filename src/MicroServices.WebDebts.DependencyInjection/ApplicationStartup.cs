using MicroServices.WebDebts.Application.Service;
using MicroServices.WebDebts.Application.Services;
using MicroServices.WebDebts.Domain.Interfaces.Repository;
using MicroServices.WebDebts.Domain.Service;
using MicroServices.WebDebts.Domain.Services;
using MicroServices.WebDebts.Infrastructure.Database.Postgres;
using MicroServices.WebDebts.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddScoped<ICardsApplicationService, CardsApplicationService>();
            services.AddScoped<IWalletApplicationService, WalletApplicationService>();
            services.AddScoped<IUserApplicationService, UserApplicationService>();
            services.AddScoped<ITokenApplicationService, TokenApplicationService>();
            services.AddScoped<ISpendingCeilingApplicationService, SpendingCeilingApplicationService>();
        }

        public static void RegisterDomain(IServiceCollection services)
        {
            services.AddScoped<IDebtsService, DebtsService>();
            services.AddScoped<ICardService, CardService>();
            services.AddScoped<IWalletService, WalletService>();
            services.AddScoped<ISpendingCeilingService, SpendingCeilingService>();
        }

        public static void RegisterInfrastructure(IServiceCollection services)
        {
            services.AddScoped<IDebtRepository, DebtRepository>();
            services.AddScoped<ICardRepository, CardRepository>();
            services.AddScoped<IWalletRepository, WalletRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ISpendingCeilingRepository, SpendingCeilingRepository>();
        }
    }
}