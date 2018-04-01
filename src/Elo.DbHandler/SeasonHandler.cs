using Elo.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Elo.DbHandler
{
    public static class SeasonHandler
    {
        public static Season AddSeason(Season season)
        {
            using (var db = new EloDbContext())
            {
                db.Seasons.Add(season);
                db.SaveChanges();
            }

            return season;
        }

        public static Season GetSeason(string name)
        {
            using (var db = new EloDbContext())
            {
                return db.Seasons.FirstOrDefault(s => s.Name == name);
            }
        }

        public static List<Season> GetAllSeasons()
        {
            using (var db = new EloDbContext())
            {
                return db.Seasons.ToList();
            }
        }

        public static List<Season> GetActiveSeasons(DateTimeOffset dateTime)
        {
            using (var db = new EloDbContext())
            {
                return db.Seasons.Where(s => s.IsActive(dateTime)).ToList();
            }
        }

        public static List<Season> GetStartedSeasons(DateTimeOffset dateTime)
        {
            using (var db = new EloDbContext())
            {
                return db.Seasons.Where(s => s.HasStarted(dateTime)).ToList();
            }
        }

        public static List<Season> GetCompletedSeasons(DateTimeOffset dateTime)
        {
            using (var db = new EloDbContext())
            {
                return db.Seasons.Where(s => s.HasEnded(dateTime)).ToList();
            }
        }
    }
}
