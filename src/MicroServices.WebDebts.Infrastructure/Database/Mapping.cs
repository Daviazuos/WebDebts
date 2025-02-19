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

            modelBuilder.Entity<Debt>().HasOne(x => x.User);

            modelBuilder.Entity<Debt>().HasOne(x => x.DebtCategory);

            modelBuilder.Entity<Installments>().HasOne(x => x.User);

            modelBuilder.Entity<Card>().HasOne(x => x.User);

            modelBuilder.Entity<Wallet>().HasOne(x => x.User);

            modelBuilder.Entity<SpendingCeiling>().HasOne(x => x.User);
            
            modelBuilder.Entity<SpendingCeiling>().HasOne(x => x.DebtCategory);
     
            modelBuilder.Entity<Goal>().HasOne(x => x.Debt);

            modelBuilder.Entity<WalletInstallments>().HasOne(d => d.User);

            modelBuilder.Entity<WalletInstallments>().HasOne(d => d.Wallet);

            modelBuilder.Entity<Debt>().HasOne(x => x.ResponsibleParty);

            modelBuilder.Entity<Wallet>().HasOne(x => x.ResponsibleParty);

            modelBuilder.Entity<DraftDebt>().HasOne(x => x.Card);

            modelBuilder.Entity<DraftDebt>().HasOne(x => x.User);
        }
    }
}
