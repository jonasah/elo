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
                    .FirstOrDefault(ps => ps.PlayerId == player.Id && ps.SeasonId == season.Id)? // zero or one
                    .PlayerRatings ?? new List<PlayerRating>();
            }
        }

        public static void DeleteRatingsAfter(int ratingId, bool deleteDefaultRatings = true)
        {
            using (var db = new EloDbContext())
            {
                db.Ratings.RemoveRange(db.Ratings.Where(r => r.Id > ratingId && (deleteDefaultRatings || r.GameId != null)));
                db.SaveChanges();
            }
        }

        public static void DeleteAllRatings()
        {
            DeleteRatingsAfter(int.MinValue);
        }

        public static List<PlayerRating> GetLatestRatingsPerPlayerSeason()
        {
            using (var db = new EloDbContext())
            {
                return db.Ratings
                    .Include(pr => pr.PlayerSeason)
                        .ThenInclude(ps => ps.Season)
                    .GroupBy(pr => pr.PlayerSeasonId)
                    .Select(g => g.OrderByDescending(pr => pr.Id).First()) // take latest rating record
                    .ToList();
            }
        }
    }
}
