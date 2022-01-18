using Microsoft.EntityFrameworkCore.Migrations;

namespace MicroServices.WebDebts.Infrastructure.Migrations
{
    public partial class ajustwallet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WalletMonthController_Wallet_WalletId",
                table: "WalletMonthController");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WalletMonthController",
                table: "WalletMonthController");

            migrationBuilder.RenameTable(
                name: "WalletMonthController",
                newName: "WalletMonthControllers");

            migrationBuilder.RenameIndex(
                name: "IX_WalletMonthController_WalletId",
                table: "WalletMonthControllers",
                newName: "IX_WalletMonthControllers_WalletId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WalletMonthControllers",
                table: "WalletMonthControllers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WalletMonthControllers_Wallet_WalletId",
                table: "WalletMonthControllers",
                column: "WalletId",
                principalTable: "Wallet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WalletMonthControllers_Wallet_WalletId",
                table: "WalletMonthControllers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WalletMonthControllers",
                table: "WalletMonthControllers");

            migrationBuilder.RenameTable(
                name: "WalletMonthControllers",
                newName: "WalletMonthController");

            migrationBuilder.RenameIndex(
                name: "IX_WalletMonthControllers_WalletId",
                table: "WalletMonthController",
                newName: "IX_WalletMonthController_WalletId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WalletMonthController",
                table: "WalletMonthController",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WalletMonthController_Wallet_WalletId",
                table: "WalletMonthController",
                column: "WalletId",
                principalTable: "Wallet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
