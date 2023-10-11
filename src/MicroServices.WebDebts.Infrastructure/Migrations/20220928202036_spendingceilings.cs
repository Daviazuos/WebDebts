using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MicroServices.WebDebts.Infrastructure.Migrations
{
    public partial class spendingceilings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WalletInstallmentType",
                table: "Wallet",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SpendingCeiling",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<decimal>(type: "numeric", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    DebtCategoryId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpendingCeiling", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpendingCeiling_DebtCategories_DebtCategoryId",
                        column: x => x.DebtCategoryId,
                        principalTable: "DebtCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SpendingCeiling_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpendingCeiling_DebtCategoryId",
                table: "SpendingCeiling",
                column: "DebtCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SpendingCeiling_UserId",
                table: "SpendingCeiling",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpendingCeiling");

            migrationBuilder.DropColumn(
                name: "WalletInstallmentType",
                table: "Wallet");
        }
    }
}
