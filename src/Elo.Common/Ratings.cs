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

                // update current ratings
                winningPlayerSeason.RatingChange = winner.Rating - winningPlayerSeason.Rating;
                winningPlayerSeason.Rating = winner.Rating;
                ++winningPlayerSeason.Wins;
                winningPlayerSeason.CurrentStreak = Math.Max(winningPlayerSeason.CurrentStreak + 1, 1);

                losingPlayerSeason.RatingChange = loser.Rating - losingPlayerSeason.Rating;
                losingPlayerSeason.Rating = loser.Rating;
                ++losingPlayerSeason.Losses;
                losingPlayerSeason.CurrentStreak = Math.Min(losingPlayerSeason.CurrentStreak - 1, -1);

                // add new ratings
                var winningPlayerRating = winningPlayerSeason.CreatePlayerRating(game.Id);
                var losingPlayerRating = losingPlayerSeason.CreatePlayerRating(game.Id);

                // update database
                PlayerHandler.UpdatePlayerSeasons(winningPlayerSeason, losingPlayerSeason);
                RatingHandler.AddRatings(winningPlayerRating, losingPlayerRating);
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

                // add default rating
                RatingHandler.AddRating(playerSeason.CreatePlayerRating());
            }

            return playerSeason;
        }
    }
}
