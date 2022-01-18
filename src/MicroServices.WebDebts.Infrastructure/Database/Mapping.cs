using MicroServices.WebDebts.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroServices.WebDebts.Infrastructure.Database
{
    public class Mapping
    {
       public static void MapDebt(ModelBuilder modelBuilder)
       {
            modelBuilder.Entity<Debt>().HasKey(k => k.Id);
           
            modelBuilder.Entity<Debt>().HasMany(d => d.Installments);

            modelBuilder.Entity<Installments>().HasOne(d => d.Debt);

            modelBuilder.Entity<Card>().HasKey(k => k.Id);

            modelBuilder.Entity<Card>()
                .HasMany(d => d.DebtValues);

            modelBuilder.Entity<Wallet>().HasKey(k => k.Id);

            modelBuilder.Entity<Wallet>().HasMany(d => d.WalletMonthControllers);

            modelBuilder.Entity<WalletMonthController>().HasOne(d => d.Wallet);
        }
    }
}
