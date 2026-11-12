using System;
using MicroServices.WebDebts.Infrastructure.Database.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace MicroServices.WebDebts.Infrastructure.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20261112150000_preserve-original-planner-budget")]
    public partial class PreserveOriginalPlannerBudget : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "OriginalBudgetedValue",
                table: "PlannerCategories",
                type: "numeric",
                nullable: true);

            migrationBuilder.Sql(
                "UPDATE \"PlannerCategories\" SET \"OriginalBudgetedValue\" = \"BudgetedValue\"");

            migrationBuilder.AlterColumn<decimal>(
                name: "OriginalBudgetedValue",
                table: "PlannerCategories",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OriginalBudgetedValue",
                table: "PlannerCategories");
        }

        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.13")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("MicroServices.WebDebts.Domain.Models.PlannerCategories", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uuid");

                b.Property<decimal>("BudgetedValue")
                    .HasColumnType("numeric");

                b.Property<DateTime>("CreatedAt")
                    .HasColumnType("timestamp without time zone");

                b.Property<Guid?>("DebtCategoryId")
                    .HasColumnType("uuid");

                b.Property<Guid?>("PlannerFrequencyId")
                    .HasColumnType("uuid");

                b.Property<decimal>("OriginalBudgetedValue")
                    .HasColumnType("numeric");

                b.HasKey("Id");
                b.HasIndex("DebtCategoryId");
                b.HasIndex("PlannerFrequencyId");
                b.ToTable("PlannerCategories");
            });
        }
    }
}
