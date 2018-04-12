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
            Console.WriteLine("Delete all ratings");
            PlayerHandler.DeleteAllPlayerSeasons();
            RatingHandler.DeleteAllRatings();

            // get all games in chronological order
            Console.WriteLine("Get games");
            var games = GameHandler.GetGamesAfter(DateTime.MinValue, SortOrder.Ascending);

            // calculate new ratings
            Console.WriteLine($"Calculate new ratings for {games.Count} games");
            games.ForEach(g => Ratings.CalculateNewRatings(g));
        }
    }
}
