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
            RatingHandler.DeleteAllRatings();
            Console.WriteLine("Delete all player stats");
            PlayerHandler.DeleteAllPlayerSeasons();

            // get all games in chronological order
            Console.WriteLine("Get games");
            var games = GameHandler.GetGamesAfter(DateTime.MinValue, SortOrder.Ascending);

            // calculate new ratings
            Console.WriteLine("Calculate new ratings");
            games.ForEach(g => Ratings.CalculateNewRatings(g));
        }
    }
}
