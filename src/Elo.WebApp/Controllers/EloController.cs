using System;
using System.Collections.Generic;
using System.Linq;
using Elo.DbHandler;
using Elo.Models;
using Elo.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Elo.WebApp.Controllers
{
    [Route("api/elo")]
    public class EloController : Controller
    {
        [HttpGet("ratings")]
        public IEnumerable<Models.Dto.PlayerRating> GetPlayerRatings()
        {
            var rank = 1;

            return PlayerHandler.GetAllPlayers()
                .OrderByDescending(p => p.CurrentRating)
                .Select(p =>
            {
                return new Models.Dto.PlayerRating
                {
                    Id = p.Id,
                    Rank = rank++,
                    Player = p.Name,
                    Rating = p.CurrentRating,
                    GamesPlayed = p.GameScores.Count,
                    Wins = p.GameScores.Count(gs => gs.Win),
                    Losses = p.GameScores.Count(gs => gs.Loss)
                };
            });
        }

        [HttpPost("gameresults")]
        public void PostGameResult([FromBody]GameResult gameResult)
        {
            try
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
                GameHandler.AddGame(new Game
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

                PlayerHandler.UpdatePlayer(winningPlayer);
                PlayerHandler.UpdatePlayer(losingPlayer);
            }
            catch (Exception /*ex*/)
            {
                return;
            }
        }

        private Player GetOrCreatePlayer(string name)
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

        private void ValidateGameResult(GameResult gameResult)
        {
            if (gameResult == null)
            {
                throw new ArgumentNullException();
            }

            if (gameResult.Winner.IsNullOrEmpty() || gameResult.Loser.IsNullOrEmpty())
            {
                throw new ArgumentException("Winner and/or loser is not set");
            }

            if (gameResult.Winner == gameResult.Loser)
            {
                throw new ArgumentException("Winner and loser cannot be the same player");
            }
        }
    }

    internal static class Extensions
    {
        public static bool IsNullOrEmpty(this string str) => string.IsNullOrEmpty(str);
    }
}
