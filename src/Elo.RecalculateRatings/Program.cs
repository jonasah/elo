using Elo.Common;
using Elo.DbHandler;
using System;
using System.Data.SqlClient;

namespace Elo.RecalculateRatings
{
    class Program
    {
        static void Main(string[] args)
        {
            // delete all ratings and stats
            RatingHandler.DeleteAllRatings();
            PlayerHandler.DeleteAllPlayerSeasons();

            // get all games in chronological order
            var games = GameHandler.GetGamesAfter(DateTime.MinValue, SortOrder.Ascending);

            // calculate new ratings
            games.ForEach(g => Ratings.CalculateNewRatings(g));
        }
    }
}
