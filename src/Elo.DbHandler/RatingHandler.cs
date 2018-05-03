using Elo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Elo.DbHandler
{
    public static class RatingHandler
    {
        public static void AddRating(PlayerRating rating)
        {
            using (var db = new EloDbContext())
            {
                db.Ratings.Add(rating);
                db.SaveChanges();
            }
        }

        public static void AddRatings(IEnumerable<PlayerRating> ratings)
        {
            using (var db = new EloDbContext())
            {
                db.Ratings.AddRange(ratings);
                db.SaveChanges();
            }
        }

        public static void AddRatings(params PlayerRating[] ratings) => AddRatings(ratings.AsEnumerable());

        public static List<PlayerRating> GetRatingsByPlayerAndSeason(Player player, Season season)
        {
            using (var db = new EloDbContext())
            {
                return db.PlayerSeasons
                    .Where(ps => ps.PlayerId == player.Id && ps.SeasonId == season.Id)
                    .Include(ps => ps.Ratings)
                        .ThenInclude(psr => psr.PlayerRating)
                    .SelectMany(ps => ps.Ratings)
                    .Select(psr => psr.PlayerRating)
                    .ToList();
            }
        }

        public static void DeleteRatingsAfter(DateTime timestamp)
        {
            using (var db = new EloDbContext())
            {
                db.Ratings.RemoveRange(db.Ratings.Where(r => r.Created > timestamp));
                db.SaveChanges();
            }
        }

        public static void DeleteAllRatings()
        {
            DeleteRatingsAfter(DateTime.MinValue);
        }

        public static void AddPlayerSeasonRatings(IEnumerable<PlayerSeasonRating> playerSeasonRatings)
        {
            using (var db = new EloDbContext())
            {
                db.PlayerSeasonRatings.AddRange(playerSeasonRatings);
                db.SaveChanges();
            }
        }

        public static void AddPlayerSeasonRatings(params PlayerSeasonRating[] playerSeasonRatings) =>
            AddPlayerSeasonRatings(playerSeasonRatings.AsEnumerable());
    }
}
