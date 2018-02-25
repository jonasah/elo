using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
                .Select(p => new Models.Dto.PlayerRating
                {
                    Id = p.Id,
                    Rank = rank++,
                    Player = p.Name,
                    Rating = Math.Round(p.CurrentRating),
                    Wins = p.Wins,
                    Losses = p.Losses,
                    Streak = p.CurrentStreak
                });
        }

        [HttpGet("players")]
        public IEnumerable<string> GetPlayers()
        {
            return PlayerHandler.GetAllPlayerNames();
        }

        [HttpGet("playerstats/{player}/h2h")]
        public IEnumerable<Head2HeadRecord> GetHead2HeadRecords([FromRoute(Name = "player")]string playerName)
        {
            var games = GameHandler.GetGamesByPlayer(playerName, SortOrder.Descending);

            return games
                .SelectMany(g => g.Scores.Where(gs => gs.Player.Name != playerName))
                .GroupBy(gs => gs.Player)
                .Select(g => new Head2HeadRecord
                {
                    Opponent = g.Key.Name,
                    Wins = g.Count(gs => gs.Loss), // player's wins are losses for opponent
                    Losses = g.Count(gs => gs.Win) // player's losses are wins for opponent
                })
                .OrderBy(h2h => h2h.Opponent);
        }

        [HttpGet("playerstats/{player}/expectedscores")]
        public IEnumerable<ExpectedScore> GetExpectedScores([FromRoute(Name = "player")]string playerName)
        {
            var players = PlayerHandler.GetAllPlayers();
            var player = players.Find(p => p.Name == playerName);
            players.Remove(player);

            var libPlayer = player.ToEloLibPlayer();

            return players
                .Select(p => new ExpectedScore
                {
                    Opponent = p.Name,
                    Score = libPlayer.ExpectedScore(p.ToEloLibPlayer())
                })
                .OrderByDescending(es => es.Score);
        }

        [HttpPost("game")]
        public bool PostGame([FromBody]GameResult gameResult)
        {
            try
            {
                CalculateNewRatings(gameResult, addGame: true);

                return true;
            }
            catch (Exception /*ex*/)
            {
                return false;
            }
        }

        [HttpGet("games")]
        public IEnumerable<Models.Dto.Game> GetGames(int page = 1, int pageSize = 20)
        {
            return GameHandler.GetGames(page, pageSize, SortOrder.Descending)
                .Select(g => new Models.Dto.Game
                {
                    Id = g.Id,
                    Winner = g.WinningGameScore.Player.Name,
                    Loser = g.LosingGameScore.Player.Name,
                    Date = $"{g.Created.ToString()} UTC"
                });
        }

        [HttpGet("games/{player}")]
        public IEnumerable<Models.Dto.Game> GetGamesByPlayer(string player, int page = 1, int pageSize = 20)
        {
            return GameHandler.GetGamesByPlayer(player, page, pageSize, SortOrder.Descending)
                .Select(g => new Models.Dto.Game
                {
                    Id = g.Id,
                    Winner = g.WinningGameScore.Player.Name,
                    Loser = g.LosingGameScore.Player.Name,
                    Date = $"{g.Created.ToString()} UTC"
                });
        }

        [HttpDelete("game/{id}")]
        public bool DeleteGame(int id)
        {
            try
            {
                // FIXME: delete game not working
                throw new NotImplementedException();

                // get the game
                var game = GameHandler.GetGame(id);

                if (game == null)
                {
                    throw new ArgumentException("No such game");
                }

                // delete the game (and its scores)
                GameHandler.DeleteGame(game);

                // delete all ratings from this game and later
                RatingHandler.DeleteRatingsAfter(game.Created);

                // get all games after the deleted game
                var games = GameHandler.GetGamesAfter(game.Created, SortOrder.Ascending);

                // recalculate ratings
                foreach (var g in games)
                {
                    CalculateNewRatings(new GameResult
                    {
                        Winner = g.WinningGameScore.Player.Name,
                        Loser = g.LosingGameScore.Player.Name
                    },
                    addGame: false);
                }

                return true;
            }
            catch (Exception /*ex*/)
            {
                return false;
            }
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

            RatingHandler.AddRatings(new Models.PlayerRating
            {
                PlayerId = winningPlayer.Id,
                Rating = winningPlayer.CurrentRating
            },
            new Models.PlayerRating
            {
                PlayerId = losingPlayer.Id,
                Rating = losingPlayer.CurrentRating
            });
        }
    }
}
