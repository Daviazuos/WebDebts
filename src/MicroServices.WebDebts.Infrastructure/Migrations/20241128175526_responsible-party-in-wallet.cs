using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MicroServices.WebDebts.Infrastructure.Migrations
{
    public partial class responsiblepartyinwallet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ResponsiblePartyId",
                table: "Wallet",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Wallet_ResponsiblePartyId",
                table: "Wallet",
                column: "ResponsiblePartyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Wallet_ResponsibleParty_ResponsiblePartyId",
                table: "Wallet",
                column: "ResponsiblePartyId",
                principalTable: "ResponsibleParty",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wallet_ResponsibleParty_ResponsiblePartyId",
                table: "Wallet");

            migrationBuilder.DropIndex(
                name: "IX_Wallet_ResponsiblePartyId",
                table: "Wallet");

            migrationBuilder.DropColumn(
                name: "ResponsiblePartyId",
                table: "Wallet");
        }
    }
}
