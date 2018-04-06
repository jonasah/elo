using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Elo.DbHandler.Migrations
{
    public partial class NoStatsInPlayerSeason : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerRatings_PlayerSeasons_PlayerSeasonId",
                table: "PlayerRatings");

            migrationBuilder.DropIndex(
                name: "IX_PlayerRatings_PlayerSeasonId",
                table: "PlayerRatings");

            migrationBuilder.DropColumn(
                name: "CurrentRating",
                table: "PlayerSeasons");

            migrationBuilder.DropColumn(
                name: "CurrentStreak",
                table: "PlayerSeasons");

            migrationBuilder.DropColumn(
                name: "Losses",
                table: "PlayerSeasons");

            migrationBuilder.DropColumn(
                name: "PlayerSeasonId",
                table: "PlayerRatings");

            migrationBuilder.RenameColumn(
                name: "Wins",
                table: "PlayerSeasons",
                newName: "CurrentPlayerRatingId");

            migrationBuilder.CreateTable(
                name: "PlayerSeasonRatings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(nullable: false, defaultValueSql: "getutcdate()"),
                    PlayerRatingId = table.Column<int>(nullable: false),
                    PlayerSeasonId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerSeasonRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerSeasonRatings_PlayerRatings_PlayerRatingId",
                        column: x => x.PlayerRatingId,
                        principalTable: "PlayerRatings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlayerSeasonRatings_PlayerSeasons_PlayerSeasonId",
                        column: x => x.PlayerSeasonId,
                        principalTable: "PlayerSeasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerSeasons_CurrentPlayerRatingId",
                table: "PlayerSeasons",
                column: "CurrentPlayerRatingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerSeasonRatings_PlayerRatingId",
                table: "PlayerSeasonRatings",
                column: "PlayerRatingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerSeasonRatings_PlayerSeasonId",
                table: "PlayerSeasonRatings",
                column: "PlayerSeasonId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerSeasons_PlayerRatings_CurrentPlayerRatingId",
                table: "PlayerSeasons",
                column: "CurrentPlayerRatingId",
                principalTable: "PlayerRatings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerSeasons_PlayerRatings_CurrentPlayerRatingId",
                table: "PlayerSeasons");

            migrationBuilder.DropTable(
                name: "PlayerSeasonRatings");

            migrationBuilder.DropIndex(
                name: "IX_PlayerSeasons_CurrentPlayerRatingId",
                table: "PlayerSeasons");

            migrationBuilder.RenameColumn(
                name: "CurrentPlayerRatingId",
                table: "PlayerSeasons",
                newName: "Wins");

            migrationBuilder.AddColumn<double>(
                name: "CurrentRating",
                table: "PlayerSeasons",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "CurrentStreak",
                table: "PlayerSeasons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Losses",
                table: "PlayerSeasons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PlayerSeasonId",
                table: "PlayerRatings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerRatings_PlayerSeasonId",
                table: "PlayerRatings",
                column: "PlayerSeasonId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerRatings_PlayerSeasons_PlayerSeasonId",
                table: "PlayerRatings",
                column: "PlayerSeasonId",
                principalTable: "PlayerSeasons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
