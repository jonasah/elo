using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Elo.Common;
using Elo.DbHandler;
using Elo.Models;
using Elo.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Elo.WebApp.Controllers
{
    [Route("api/elo")]
    public class EloController : Controller
    {
        [HttpGet("ratings/{season}")]
        public IEnumerable<Models.Dto.PlayerRating> GetPlayerRatings([FromRoute(Name = "season")]string seasonName, int minGamesPlayed = 1)
        {
            var rank = 1;

            return PlayerHandler.GetAllPlayerSeasons(seasonName)
                .Where(p => p.CurrentPlayerRating.GamesPlayed >= minGamesPlayed)
                .OrderByDescending(ps => ps.CurrentPlayerRating.Rating)
                .Select(ps => new Models.Dto.PlayerRating
                {
                    Id = ps.Id,
                    Rank = rank++,
                    Player = ps.Player.Name,
                    Rating = Math.Round(ps.CurrentPlayerRating.Rating),
                    Wins = ps.CurrentPlayerRating.Wins,
                    Losses = ps.CurrentPlayerRating.Losses,
                    Streak = ps.CurrentPlayerRating.CurrentStreak,
                    RatingChange = Math.Round(ps.CurrentPlayerRating.RatingChange)
                });
        }

        [HttpGet("players")]
        public IEnumerable<string> GetPlayers()
        {
            return PlayerHandler.GetAllPlayerNames();
        }

        [HttpGet("playerstats/{player}/{season}/h2h")]
        public IEnumerable<Head2HeadRecord> GetHead2HeadRecords(
            [FromRoute(Name = "player")]string playerName,
            [FromRoute(Name = "season")]string seasonName)
        {
            var player = PlayerHandler.GetPlayerByName(playerName);
            var season = SeasonHandler.GetSeason(seasonName);

            if (player == null || season == null)
            {
                return new Head2HeadRecord[0];
            }

            var games = GameHandler
                .GetGamesByPlayer(playerName, SortOrder.Descending)
                .FindAll(g => season.IsActive(g.Created));

            var ratings = RatingHandler.GetRatingsByPlayerAndSeason(player, season)
                .Where(pr => pr.GameId != null)
                .ToDictionary(pr => pr.GameId);

            return games
                .SelectMany(g => g.Scores.Where(gs => gs.Player.Name != playerName))
                .GroupBy(gs => gs.Player) // group by opponent
                .Select(g => new Head2HeadRecord
                {
                    Opponent = g.Key.Name,
                    Wins = g.Count(gs => gs.Loss), // player's wins are losses for opponent
                    Losses = g.Count(gs => gs.Win), // player's losses are wins for opponent
                    RatingChange = Math.Round(g.Sum(gs => ratings[gs.GameId].RatingChange)) // summing player's rating changes
                })
                .OrderBy(h2h => h2h.Opponent);
        }

        [HttpGet("playerstats/{player}/{season}/expectedscores")]
        public IEnumerable<ExpectedScore> GetExpectedScores(
            [FromRoute(Name = "player")]string playerName,
            [FromRoute(Name = "season")]string seasonName)
        {
            var playerSeasons = PlayerHandler.GetAllPlayerSeasons(seasonName).ToList();
            var myPlayerSeason = playerSeasons.Find(ps => ps.Player.Name == playerName);

            if (myPlayerSeason == null)
            {
                // player did not compete in this season
                return new ExpectedScore[0];
            }

            playerSeasons.Remove(myPlayerSeason);

            var libPlayer = myPlayerSeason.ToEloLibPlayer();

            return playerSeasons
                .Select(ps => new ExpectedScore
                {
                    Opponent = ps.Player.Name,
                    Score = libPlayer.ExpectedScore(ps.ToEloLibPlayer())
                })
                .OrderByDescending(es => es.Score);
        }

        [HttpPost("game")]
        public bool PostGame([FromBody]GameResult gameResult)
        {
            try
            {
                ValidateGameResult(gameResult);

                var winningPlayer = GetOrCreatePlayer(gameResult.Winner);
                var losingPlayer = GetOrCreatePlayer(gameResult.Loser);

                var game = GameHandler.AddGame(new Models.Game
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

                Ratings.CalculateNewRatings(game);

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
                games.ForEach(g => Ratings.CalculateNewRatings(g));

                return true;
            }
            catch (Exception /*ex*/)
            {
                return false;
            }
        }

        [HttpPost("season")]
        public bool AddSeason([FromBody]SeasonPost seasonPost)
        {
            SeasonHandler.AddSeason(new Season
            {
                Name = seasonPost.Name,
                StartDate = seasonPost.StartDate,
                EndDate = seasonPost.EndDate
            });

            return true;
        }

        [HttpGet("activeseasons")]
        public IEnumerable<string> GetActiveSeasons()
        {
            return SeasonHandler
                .GetActiveSeasons(DateTimeOffset.UtcNow)
                .OrderBy(s => s.StartDate)
                .Select(s => s.Name);
        }

        [HttpGet("activeseasons/{player}")]
        public IEnumerable<string> GetActiveSeasonsByPlayer([FromRoute(Name = "player")]string playerName)
        {
            return SeasonHandler
                .GetActiveSeasonsByPlayer(DateTimeOffset.UtcNow, playerName)
                .OrderBy(s => s.StartDate)
                .Select(s => s.Name);
        }

        [HttpGet("startedseasons")]
        public IEnumerable<string> GetStartedSeasons()
        {
            return SeasonHandler
                .GetStartedSeasons(DateTimeOffset.UtcNow)
                .OrderBy(s => s.StartDate)
                .Select(s => s.Name);
        }

        [HttpGet("startedseasons/{player}")]
        public IEnumerable<string> GetStartedSeasonsByPlayer([FromRoute(Name = "player")]string playerName)
        {
            return SeasonHandler
                .GetStartedSeasonsByPlayer(DateTimeOffset.UtcNow, playerName)
                .OrderBy(s => s.StartDate)
                .Select(s => s.Name);
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
