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
                    .Include(ps => ps.PlayerRatings)
                    .First(ps => ps.PlayerId == player.Id && ps.SeasonId == season.Id) // should only be one
                    .PlayerRatings;
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
    }
}
