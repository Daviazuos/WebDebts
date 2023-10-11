using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MicroServices.WebDebts.Infrastructure.Migrations
{
    public partial class walletid_in_installment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "WalletMonthControllerId",
                table: "Installments",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "WalletMonthController",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Month = table.Column<int>(nullable: false),
                    UpdatedValue = table.Column<decimal>(nullable: false),
                    WalletId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalletMonthController", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WalletMonthController_Wallet_WalletId",
                        column: x => x.WalletId,
                        principalTable: "Wallet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WalletMonthController_WalletId",
                table: "WalletMonthController",
                column: "WalletId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WalletMonthController");

            migrationBuilder.DropColumn(
                name: "WalletMonthControllerId",
                table: "Installments");
        }
    }
}
