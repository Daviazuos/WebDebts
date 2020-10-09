﻿using MicroServices.WebDebts.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroServices.WebDebts.Infrastructure.Database
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Debt> Debt { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Mapping.MapDebt(modelBuilder);
        }
    }
}