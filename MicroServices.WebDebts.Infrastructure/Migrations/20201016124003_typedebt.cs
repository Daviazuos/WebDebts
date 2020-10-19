using Microsoft.EntityFrameworkCore.Migrations;

namespace MicroServices.WebDebts.Infrastructure.Migrations
{
    public partial class typedebt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DebtInstallmentType",
                table: "Debt",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DebtInstallmentType",
                table: "Debt");
        }
    }
}
