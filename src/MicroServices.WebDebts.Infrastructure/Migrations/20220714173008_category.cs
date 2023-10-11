using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MicroServices.WebDebts.Infrastructure.Migrations
{
    public partial class category : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DebtCategoryId",
                table: "Debt",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DebtCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DebtCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DebtCategories_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Debt_DebtCategoryId",
                table: "Debt",
                column: "DebtCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_DebtCategories_UserId",
                table: "DebtCategories",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Debt_DebtCategories_DebtCategoryId",
                table: "Debt",
                column: "DebtCategoryId",
                principalTable: "DebtCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Debt_DebtCategories_DebtCategoryId",
                table: "Debt");

            migrationBuilder.DropTable(
                name: "DebtCategories");

            migrationBuilder.DropIndex(
                name: "IX_Debt_DebtCategoryId",
                table: "Debt");

            migrationBuilder.DropColumn(
                name: "DebtCategoryId",
                table: "Debt");
        }
    }
}
