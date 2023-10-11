using MicroServices.WebDebts.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroServices.WebDebts.Infrastructure.Database.Postgres
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Debt> Debt { get; set; }
        public DbSet<DebtCategory> DebtCategories { get; set; }
        public DbSet<Installments> Installments { get; set; }
        public DbSet<Card> Card { get; set; }
        public DbSet<Wallet> Wallet{ get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<SpendingCeiling> SpendingCeiling { get; set; }
        public DbSet<Goal> Goal { get; set; }
        public DbSet<WalletInstallments> WalletInstallments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Mapping.MapDebt(modelBuilder);
        }
    }
}