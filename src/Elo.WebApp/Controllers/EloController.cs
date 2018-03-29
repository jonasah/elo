using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Elo.Common;
using Elo.DbHandler;
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
                Ratings.CalculateNewRatings(gameResult, addGame: true);

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
                    Ratings.CalculateNewRatings(new GameResult
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
    }
}
