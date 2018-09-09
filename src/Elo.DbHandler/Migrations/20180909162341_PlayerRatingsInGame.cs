using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Elo.DbHandler.Migrations
{
    public partial class PlayerRatingsInGame : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerRatings_Games_GameId",
                table: "PlayerRatings");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerRatings_Games_GameId",
                table: "PlayerRatings",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerRatings_Games_GameId",
                table: "PlayerRatings");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerRatings_Games_GameId",
                table: "PlayerRatings",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
