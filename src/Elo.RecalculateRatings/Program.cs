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
            var completed = 0;

            foreach (var game in games)
            {
                Ratings.CalculateNewRatings(game);

                ++completed;
                Console.Write($"\rProgress: {completed}/{games.Count} ({completed / (double)games.Count:P1})");
            }

            Console.WriteLine();
        }
    }
}
