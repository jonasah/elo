using Elo.Common;
using Elo.DbHandler;
using Elo.Models.Dto;
using System;
using System.Data.SqlClient;

namespace Elo.RecalculateRatings
{
    class Program
    {
        static void Main(string[] args)
        {
            // delete all ratings
            RatingHandler.DeleteRatingsAfter(DateTime.MinValue);

            var players = PlayerHandler.GetAllPlayers();

            // set default stats for all players
            foreach (var player in players)
            {
                var defaultRating = new Models.PlayerRating
                {
                    PlayerId = player.Id,
                    Rating = Lib.Settings.DefaultRating
                };

                player.CurrentRating = Lib.Settings.DefaultRating;
                player.Wins = 0;
                player.Losses = 0;
                player.CurrentStreak = 0;

                PlayerHandler.UpdatePlayer(player);
                RatingHandler.AddRating(defaultRating);
            }

            // get all games in chronological order
            var games = GameHandler.GetGamesAfter(DateTime.MinValue, SortOrder.Ascending);

            // calculate new ratings
            foreach (var game in games)
            {
                Ratings.CalculateNewRatings(new GameResult
                {
                    Winner = game.WinningGameScore.Player.Name,
                    Loser = game.LosingGameScore.Player.Name
                },
                addGame: false);
            }
        }
    }
}
