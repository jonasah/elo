using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Elo.DbHandler.Migrations
{
    public partial class GameInPlayerRating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GameId",
                table: "PlayerRatings",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerRatings_GameId",
                table: "PlayerRatings",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerRatings_Games_GameId",
                table: "PlayerRatings",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerRatings_Games_GameId",
                table: "PlayerRatings");

            migrationBuilder.DropIndex(
                name: "IX_PlayerRatings_GameId",
                table: "PlayerRatings");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "PlayerRatings");
        }
    }
}
