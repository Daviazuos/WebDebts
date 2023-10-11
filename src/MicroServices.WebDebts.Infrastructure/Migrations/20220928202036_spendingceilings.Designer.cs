﻿// <auto-generated />
using System;
using MicroServices.WebDebts.Infrastructure.Database.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace MicroServices.WebDebts.Infrastructure.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20220928202036_spendingceilings")]
    partial class spendingceilings
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.13")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("MicroServices.WebDebts.Domain.Models.Card", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("ClosureDate")
                        .HasColumnType("integer");

                    b.Property<string>("Color")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("DueDate")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Card");
                });

            modelBuilder.Entity("MicroServices.WebDebts.Domain.Models.Debt", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CardId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid?>("DebtCategoryId")
                        .HasColumnType("uuid");

                    b.Property<int>("DebtInstallmentType")
                        .HasColumnType("integer");

                    b.Property<int>("DebtType")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("NumberOfInstallments")
                        .HasColumnType("integer");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("Value")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("CardId");

                    b.HasIndex("DebtCategoryId");

                    b.HasIndex("UserId");

                    b.ToTable("Debt");
                });

            modelBuilder.Entity("MicroServices.WebDebts.Domain.Models.DebtCategory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("DebtCategories");
                });

            modelBuilder.Entity("MicroServices.WebDebts.Domain.Models.Installments", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid?>("DebtId")
                        .HasColumnType("uuid");

                    b.Property<int>("InstallmentNumber")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("PaymentDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("Value")
                        .HasColumnType("numeric");

                    b.Property<Guid?>("WalletMonthControllerId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("DebtId");

                    b.HasIndex("UserId");

                    b.ToTable("Installments");
                });

            modelBuilder.Entity("MicroServices.WebDebts.Domain.Models.SpendingCeiling", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid?>("DebtCategoryId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("Value")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("DebtCategoryId");

                    b.HasIndex("UserId");

                    b.ToTable("SpendingCeiling");
                });

            modelBuilder.Entity("MicroServices.WebDebts.Domain.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Document")
                        .HasColumnType("text");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("MicroServices.WebDebts.Domain.Models.Wallet", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("FinishAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid?>("HistoryId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<DateTime?>("StartAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("Value")
                        .HasColumnType("numeric");

                    b.Property<int>("WalletInstallmentType")
                        .HasColumnType("integer");

                    b.Property<int>("WalletStatus")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Wallet");
                });

            modelBuilder.Entity("MicroServices.WebDebts.Domain.Models.Card", b =>
                {
                    b.HasOne("MicroServices.WebDebts.Domain.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MicroServices.WebDebts.Domain.Models.Debt", b =>
                {
                    b.HasOne("MicroServices.WebDebts.Domain.Models.Card", "Card")
                        .WithMany("DebtValues")
                        .HasForeignKey("CardId");

                    b.HasOne("MicroServices.WebDebts.Domain.Models.DebtCategory", "DebtCategory")
                        .WithMany()
                        .HasForeignKey("DebtCategoryId");

                    b.HasOne("MicroServices.WebDebts.Domain.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("Card");

                    b.Navigation("DebtCategory");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MicroServices.WebDebts.Domain.Models.DebtCategory", b =>
                {
                    b.HasOne("MicroServices.WebDebts.Domain.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MicroServices.WebDebts.Domain.Models.Installments", b =>
                {
                    b.HasOne("MicroServices.WebDebts.Domain.Models.Debt", "Debt")
                        .WithMany("Installments")
                        .HasForeignKey("DebtId");

                    b.HasOne("MicroServices.WebDebts.Domain.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("Debt");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MicroServices.WebDebts.Domain.Models.SpendingCeiling", b =>
                {
                    b.HasOne("MicroServices.WebDebts.Domain.Models.DebtCategory", "DebtCategory")
                        .WithMany()
                        .HasForeignKey("DebtCategoryId");

                    b.HasOne("MicroServices.WebDebts.Domain.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("DebtCategory");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MicroServices.WebDebts.Domain.Models.Wallet", b =>
                {
                    b.HasOne("MicroServices.WebDebts.Domain.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MicroServices.WebDebts.Domain.Models.Card", b =>
                {
                    b.Navigation("DebtValues");
                });

            modelBuilder.Entity("MicroServices.WebDebts.Domain.Models.Debt", b =>
                {
                    b.Navigation("Installments");
                });
#pragma warning restore 612, 618
        }
    }
}
