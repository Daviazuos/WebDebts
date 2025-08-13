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

            modelBuilder.Entity<Planner>().HasKey(k => k.Id);

            modelBuilder.Entity<Planner>().HasOne(p => p.User);
            modelBuilder.Entity<Planner>().HasMany(p => p.PlannerFrequencies);
            modelBuilder.Entity<PlannerFrequency>().HasKey(k => k.Id);
            modelBuilder.Entity<PlannerFrequency>().HasMany(pf => pf.PlannerCategories);
            modelBuilder.Entity<PlannerCategories>().HasKey(k => k.Id);
            modelBuilder.Entity<PlannerCategories>().HasOne(pc => pc.DebtCategory);
        }
    }
}
