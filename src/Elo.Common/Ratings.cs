using Elo.DbHandler;
using Elo.Models;
using Elo.Models.Dto;
using System;
using System.Collections.Generic;

namespace Elo.Common
{
    public static class Ratings
    {
        public static void CalculateNewRatings(GameResult gameResult, bool addGame)
        {
            ValidateGameResult(gameResult);

            // fetch players from database
            var winningPlayer = GetOrCreatePlayer(gameResult.Winner);
            var losingPlayer = GetOrCreatePlayer(gameResult.Loser);

            // convert to Elo.Lib.Players and calculate new ratings
            var p1 = winningPlayer.ToEloLibPlayer();
            var p2 = losingPlayer.ToEloLibPlayer();
            p1.WinsAgainst(p2);

            // update stats
            winningPlayer.CurrentRating = p1.Rating;
            ++winningPlayer.Wins;
            winningPlayer.CurrentStreak = Math.Max(winningPlayer.CurrentStreak + 1, 1);

            losingPlayer.CurrentRating = p2.Rating;
            ++losingPlayer.Losses;
            losingPlayer.CurrentStreak = Math.Min(losingPlayer.CurrentStreak - 1, -1);

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

            PlayerHandler.UpdatePlayers(winningPlayer, losingPlayer);
            RatingHandler.AddRatings(winningPlayer.CreatePlayerRating(), losingPlayer.CreatePlayerRating());
        }

        private static Player GetOrCreatePlayer(string name)
        {
            var player = PlayerHandler.GetPlayerByName(name);

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
    }
}
