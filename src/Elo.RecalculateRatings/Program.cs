using Elo.DbHandler;
using Elo.Models;
using Elo.Models.Dto;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Elo.RecalculateRatings
{
    class Program
    {
        private static Player GetOrCreatePlayer(string name)
        {
            var player = PlayerHandler.GetPlayerByName(name, includeGameScores: false);

            if (player == null)
            {
                player = PlayerHandler.AddPlayer(new Player
                {
                    Name = name
                });
            }

            return player;
        }

        private static void ValidateGameResult(GameResult gameResult)
        {
            if (gameResult == null)
            {
                throw new ArgumentNullException();
            }

            if (string.IsNullOrEmpty(gameResult.Winner) || string.IsNullOrEmpty(gameResult.Loser))
            {
                throw new ArgumentException("Winner and/or loser is not set");
            }

            if (gameResult.Winner == gameResult.Loser)
            {
                throw new ArgumentException("Winner and loser cannot be the same player");
            }
        }

        private static void CalculateNewRatings(GameResult gameResult, bool addGame)
        {
            ValidateGameResult(gameResult);

            // fetch players from database
            var winningPlayer = GetOrCreatePlayer(gameResult.Winner);
            var losingPlayer = GetOrCreatePlayer(gameResult.Loser);

            // convert to Elo.Lib.Players and calculate new ratings
            var p1 = winningPlayer.ToEloLibPlayer();
            var p2 = losingPlayer.ToEloLibPlayer();
            p1.WinsAgainst(p2);

            winningPlayer.CurrentRating = p1.Rating;
            losingPlayer.CurrentRating = p2.Rating;

            // update database
            if (addGame)
            {
                GameHandler.AddGame(new Models.Game
                {
                    Scores = new List<GameScore>
                    {
                        new GameScore
                        {
                            PlayerId = winningPlayer.Id,
                            Score = 1.0
                        },
                        new GameScore
                        {
                            PlayerId = losingPlayer.Id,
                            Score = 0.0
                        }
                    }
                });
            }

            RatingHandler.AddRating(winningPlayer.Ratings.Last());
            RatingHandler.AddRating(losingPlayer.Ratings.Last());
        }

        static void Main(string[] args)
        {
            // delete all ratings
            RatingHandler.DeleteRatingsAfter(DateTime.MinValue);

            var players = PlayerHandler.GetAllPlayers(includeGameScores: false);

            // add default ratings for all players
            foreach (var player in players)
            {
                player.CurrentRating = Lib.Settings.DefaultRating;
                PlayerHandler.UpdatePlayer(player);
            }

            // get all games
            var games = GameHandler.GetGamesAfter(DateTime.MinValue, SortOrder.Ascending);

            foreach (var game in games)
            {
                CalculateNewRatings(new GameResult
                {
                    Winner = game.WinningGameScore.Player.Name,
                    Loser = game.LosingGameScore.Player.Name
                },
                addGame: false);
            }
        }
    }
}
