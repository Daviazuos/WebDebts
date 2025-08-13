using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MicroServices.WebDebts.Infrastructure.Migrations
{
    public partial class ajustaplanner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Planners_DebtCategories_DebtCategoryId",
                table: "Planners");

            migrationBuilder.DropIndex(
                name: "IX_Planners_DebtCategoryId",
                table: "Planners");

            migrationBuilder.DropColumn(
                name: "BudgetedValue",
                table: "Planners");

            migrationBuilder.DropColumn(
                name: "DebtCategoryId",
                table: "Planners");

            migrationBuilder.AddColumn<int>(
                name: "Month",
                table: "Planners",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "Planners",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PlannerFrequency",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FrequencyNumber = table.Column<int>(type: "integer", nullable: false),
                    PlannerId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlannerFrequency", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlannerFrequency_Planners_PlannerId",
                        column: x => x.PlannerId,
                        principalTable: "Planners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlannerCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DebtCategoryId = table.Column<Guid>(type: "uuid", nullable: true),
                    BudgetedValue = table.Column<decimal>(type: "numeric", nullable: false),
                    PlannerFrequencyId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlannerCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlannerCategories_DebtCategories_DebtCategoryId",
                        column: x => x.DebtCategoryId,
                        principalTable: "DebtCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlannerCategories_PlannerFrequency_PlannerFrequencyId",
                        column: x => x.PlannerFrequencyId,
                        principalTable: "PlannerFrequency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlannerCategories_DebtCategoryId",
                table: "PlannerCategories",
                column: "DebtCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PlannerCategories_PlannerFrequencyId",
                table: "PlannerCategories",
                column: "PlannerFrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_PlannerFrequency_PlannerId",
                table: "PlannerFrequency",
                column: "PlannerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlannerCategories");

            migrationBuilder.DropTable(
                name: "PlannerFrequency");

            migrationBuilder.DropColumn(
                name: "Month",
                table: "Planners");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "Planners");

            migrationBuilder.AddColumn<decimal>(
                name: "BudgetedValue",
                table: "Planners",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "DebtCategoryId",
                table: "Planners",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Planners_DebtCategoryId",
                table: "Planners",
                column: "DebtCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Planners_DebtCategories_DebtCategoryId",
                table: "Planners",
                column: "DebtCategoryId",
                principalTable: "DebtCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
