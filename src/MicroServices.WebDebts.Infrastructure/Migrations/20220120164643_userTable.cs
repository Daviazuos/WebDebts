using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MicroServices.WebDebts.Infrastructure.Migrations
{
    public partial class userTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "WalletMonthControllers",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Wallet",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Installments",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Debt",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Card",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    Document = table.Column<string>(type: "text", nullable: true),
                    Username = table.Column<string>(type: "text", nullable: true),
                    Password = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WalletMonthControllers_UserId",
                table: "WalletMonthControllers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Wallet_UserId",
                table: "Wallet",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Installments_UserId",
                table: "Installments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Debt_UserId",
                table: "Debt",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Card_UserId",
                table: "Card",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Card_User_UserId",
                table: "Card",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Debt_User_UserId",
                table: "Debt",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Installments_User_UserId",
                table: "Installments",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Wallet_User_UserId",
                table: "Wallet",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WalletMonthControllers_User_UserId",
                table: "WalletMonthControllers",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Card_User_UserId",
                table: "Card");

            migrationBuilder.DropForeignKey(
                name: "FK_Debt_User_UserId",
                table: "Debt");

            migrationBuilder.DropForeignKey(
                name: "FK_Installments_User_UserId",
                table: "Installments");

            migrationBuilder.DropForeignKey(
                name: "FK_Wallet_User_UserId",
                table: "Wallet");

            migrationBuilder.DropForeignKey(
                name: "FK_WalletMonthControllers_User_UserId",
                table: "WalletMonthControllers");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropIndex(
                name: "IX_WalletMonthControllers_UserId",
                table: "WalletMonthControllers");

            migrationBuilder.DropIndex(
                name: "IX_Wallet_UserId",
                table: "Wallet");

            migrationBuilder.DropIndex(
                name: "IX_Installments_UserId",
                table: "Installments");

            migrationBuilder.DropIndex(
                name: "IX_Debt_UserId",
                table: "Debt");

            migrationBuilder.DropIndex(
                name: "IX_Card_UserId",
                table: "Card");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "WalletMonthControllers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Wallet");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Installments");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Debt");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Card");
        }
    }
}
