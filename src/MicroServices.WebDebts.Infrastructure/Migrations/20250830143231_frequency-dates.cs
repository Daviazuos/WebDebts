using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MicroServices.WebDebts.Infrastructure.Migrations
{
    public partial class frequencydates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "End",
                table: "PlannerFrequency",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Start",
                table: "PlannerFrequency",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "End",
                table: "PlannerFrequency");

            migrationBuilder.DropColumn(
                name: "Start",
                table: "PlannerFrequency");
        }
    }
}
