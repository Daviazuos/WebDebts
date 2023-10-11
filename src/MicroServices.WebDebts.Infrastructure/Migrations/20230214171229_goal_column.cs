using Microsoft.EntityFrameworkCore.Migrations;

namespace MicroServices.WebDebts.Infrastructure.Migrations
{
    public partial class goal_column : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsGoal",
                table: "Debt",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsGoal",
                table: "Debt");
        }
    }
}
