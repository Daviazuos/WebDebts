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

            modelBuilder.Entity<Card>().HasMany(d => d.DebtValues);

            modelBuilder.Entity<Wallet>().HasKey(k => k.Id);

            modelBuilder.Entity<Wallet>().HasMany(d => d.WalletMonthControllers);

            modelBuilder.Entity<WalletMonthController>().HasOne(d => d.Wallet);

            modelBuilder.Entity<Debt>().HasOne(x => x.User);

            modelBuilder.Entity<Installments>().HasOne(x => x.User);

            modelBuilder.Entity<Card>().HasOne(x => x.User);

            modelBuilder.Entity<Wallet>().HasOne(x => x.User);

            modelBuilder.Entity<WalletMonthController>().HasOne(x => x.User);
        }
    }
}
