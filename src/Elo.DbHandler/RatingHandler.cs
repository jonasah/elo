﻿using Elo.Models;
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
