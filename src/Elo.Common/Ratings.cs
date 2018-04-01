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
                var p1 = winningPlayerSeason.ToEloLibPlayer();
                var p2 = losingPlayerSeason.ToEloLibPlayer();
                p1.WinsAgainst(p2);

                // update stats
                winningPlayerSeason.CurrentRating = p1.Rating;
                ++winningPlayerSeason.Wins;
                winningPlayerSeason.CurrentStreak = Math.Max(winningPlayerSeason.CurrentStreak + 1, 1);

                losingPlayerSeason.CurrentRating = p2.Rating;
                ++losingPlayerSeason.Losses;
                losingPlayerSeason.CurrentStreak = Math.Min(losingPlayerSeason.CurrentStreak - 1, -1);

                PlayerHandler.UpdatePlayerSeasons(winningPlayerSeason, losingPlayerSeason);
                RatingHandler.AddRatings(winningPlayerSeason.CreatePlayerRating(), losingPlayerSeason.CreatePlayerRating());
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
                    SeasonId = season.Id
                });

                RatingHandler.AddRating(playerSeason.CreatePlayerRating());
            }

            return playerSeason;
        }
    }
}
