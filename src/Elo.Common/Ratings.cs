using Elo.DbHandler;
using Elo.Models;
using System;

namespace Elo.Common
{
    public static class Ratings
    {
        public static void CalculateNewRatings(Game game)
        {
            // fetch players from database
            var winningPlayer = PlayerHandler.GetPlayerById(game.WinningGameScore.PlayerId);
            var losingPlayer = PlayerHandler.GetPlayerById(game.LosingGameScore.PlayerId);

            // fetch active seasons
            var activeSeasons = SeasonHandler.GetActiveSeasons(game.Created);

            foreach (var season in activeSeasons)
            {
                var winningPlayerSeason = GetOrCreatePlayerSeason(winningPlayer, season);
                var losingPlayerSeason = GetOrCreatePlayerSeason(losingPlayer, season);

                // convert to Elo.Lib.Players and calculate new ratings
                var winner = winningPlayerSeason.ToEloLibPlayer();
                var loser = losingPlayerSeason.ToEloLibPlayer();
                winner.WinsAgainst(loser);

                // add new ratings
                var winningPlayerRating = new PlayerRating
                {
                    GameId = game.Id,
                    Rating = winner.Rating,
                    RatingChange = winner.Rating - winningPlayerSeason.CurrentPlayerRating.Rating,
                    Wins = winningPlayerSeason.CurrentPlayerRating.Wins + 1,
                    Losses = winningPlayerSeason.CurrentPlayerRating.Losses,
                    CurrentStreak = Math.Max(winningPlayerSeason.CurrentPlayerRating.CurrentStreak + 1, 1)
                };
                var losingPlayerRating = new PlayerRating
                {
                    GameId = game.Id,
                    Rating = loser.Rating,
                    RatingChange = loser.Rating - losingPlayerSeason.CurrentPlayerRating.Rating,
                    Wins = losingPlayerSeason.CurrentPlayerRating.Wins,
                    Losses = losingPlayerSeason.CurrentPlayerRating.Losses + 1,
                    CurrentStreak = Math.Min(losingPlayerSeason.CurrentPlayerRating.CurrentStreak - 1, -1)
                };

                RatingHandler.AddRatings(winningPlayerRating, losingPlayerRating);

                winningPlayerSeason.CurrentPlayerRatingId = winningPlayerRating.Id;
                winningPlayerSeason.CurrentPlayerRating = null;
                losingPlayerSeason.CurrentPlayerRatingId = losingPlayerRating.Id;
                losingPlayerSeason.CurrentPlayerRating = null;

                PlayerHandler.UpdatePlayerSeasons(winningPlayerSeason, losingPlayerSeason);

                RatingHandler.AddPlayerSeasonRatings(
                    new PlayerSeasonRating
                    {
                        PlayerSeasonId = winningPlayerSeason.Id,
                        PlayerRatingId = winningPlayerSeason.CurrentPlayerRatingId
                    },
                    new PlayerSeasonRating
                    {
                        PlayerSeasonId = losingPlayerSeason.Id,
                        PlayerRatingId = losingPlayerSeason.CurrentPlayerRatingId
                    }
                );
            }
        }

        private static PlayerSeason GetOrCreatePlayerSeason(Player player, Season season)
        {
            var playerSeason = player.Seasons.Find(ps => ps.SeasonId == season.Id);

            if (playerSeason == null)
            {
                playerSeason = PlayerHandler.AddPlayerSeason(new PlayerSeason
                {
                    PlayerId = player.Id,
                    SeasonId = season.Id,
                    CurrentPlayerRating = new PlayerRating()
                });

                RatingHandler.AddPlayerSeasonRatings(new PlayerSeasonRating
                {
                    PlayerSeasonId = playerSeason.Id,
                    PlayerRatingId = playerSeason.CurrentPlayerRatingId
                });
            }

            return playerSeason;
        }
    }
}
