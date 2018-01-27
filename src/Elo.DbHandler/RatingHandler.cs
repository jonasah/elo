using System;
using System.Linq;

namespace Elo.DbHandler
{
    public static class RatingHandler
    {
        public static void DeleteRatingsAfter(DateTime timestamp)
        {
            using (var db = new EloDbContext())
            {
                db.Ratings.RemoveRange(db.Ratings.Where(r => r.Created > timestamp));
                db.SaveChanges();
            }
        }
    }
}
