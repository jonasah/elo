using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Elo.DbHandler.Migrations
{
    public partial class PlayerSeasons : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerRatings_Players_PlayerId",
                table: "PlayerRatings");

            migrationBuilder.DropColumn(
                name: "CurrentRating",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "CurrentStreak",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Losses",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Wins",
                table: "Players");

            migrationBuilder.RenameColumn(
                name: "PlayerId",
                table: "PlayerRatings",
                newName: "PlayerSeasonId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerRatings_PlayerId",
                table: "PlayerRatings",
                newName: "IX_PlayerRatings_PlayerSeasonId");

            migrationBuilder.CreateTable(
                name: "PlayerSeasons",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(nullable: false, defaultValueSql: "getutcdate()"),
                    CurrentRating = table.Column<double>(nullable: false),
                    CurrentStreak = table.Column<int>(nullable: false),
                    Losses = table.Column<int>(nullable: false),
                    PlayerId = table.Column<int>(nullable: false),
                    SeasonId = table.Column<int>(nullable: false),
                    Wins = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerSeasons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerSeasons_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerSeasons_Seasons_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerSeasons_PlayerId",
                table: "PlayerSeasons",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerSeasons_SeasonId",
                table: "PlayerSeasons",
                column: "SeasonId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerRatings_PlayerSeasons_PlayerSeasonId",
                table: "PlayerRatings",
                column: "PlayerSeasonId",
                principalTable: "PlayerSeasons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerRatings_PlayerSeasons_PlayerSeasonId",
                table: "PlayerRatings");

            migrationBuilder.DropTable(
                name: "PlayerSeasons");

            migrationBuilder.RenameColumn(
                name: "PlayerSeasonId",
                table: "PlayerRatings",
                newName: "PlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerRatings_PlayerSeasonId",
                table: "PlayerRatings",
                newName: "IX_PlayerRatings_PlayerId");

            migrationBuilder.AddColumn<double>(
                name: "CurrentRating",
                table: "Players",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "CurrentStreak",
                table: "Players",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Losses",
                table: "Players",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Wins",
                table: "Players",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerRatings_Players_PlayerId",
                table: "PlayerRatings",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
