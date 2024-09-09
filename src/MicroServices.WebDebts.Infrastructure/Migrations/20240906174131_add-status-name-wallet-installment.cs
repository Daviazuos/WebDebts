using Microsoft.EntityFrameworkCore.Migrations;

namespace MicroServices.WebDebts.Infrastructure.Migrations
{
    public partial class addstatusnamewalletinstallment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "WalletInstallments",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ReceivedStatus",
                table: "WalletInstallments",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "WalletInstallments");

            migrationBuilder.DropColumn(
                name: "ReceivedStatus",
                table: "WalletInstallments");
        }
    }
}
