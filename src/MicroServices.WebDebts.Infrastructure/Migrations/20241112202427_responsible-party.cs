using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MicroServices.WebDebts.Infrastructure.Migrations
{
    public partial class responsibleparty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ResponsiblePartyId",
                table: "Debt",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ResponsibleParty",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResponsibleParty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResponsibleParty_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Debt_ResponsiblePartyId",
                table: "Debt",
                column: "ResponsiblePartyId");

            migrationBuilder.CreateIndex(
                name: "IX_ResponsibleParty_UserId",
                table: "ResponsibleParty",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Debt_ResponsibleParty_ResponsiblePartyId",
                table: "Debt",
                column: "ResponsiblePartyId",
                principalTable: "ResponsibleParty",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Debt_ResponsibleParty_ResponsiblePartyId",
                table: "Debt");

            migrationBuilder.DropTable(
                name: "ResponsibleParty");

            migrationBuilder.DropIndex(
                name: "IX_Debt_ResponsiblePartyId",
                table: "Debt");

            migrationBuilder.DropColumn(
                name: "ResponsiblePartyId",
                table: "Debt");
        }
    }
}
