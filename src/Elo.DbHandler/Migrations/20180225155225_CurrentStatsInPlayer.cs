using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Elo.DbHandler.Migrations
{
    public partial class CurrentStatsInPlayer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
