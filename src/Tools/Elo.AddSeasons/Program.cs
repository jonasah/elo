using Elo.DbHandler;
using Elo.Models;
using System;

namespace Elo.AddSeasons
{
    class Program
    {
        static void Main(string[] args)
        {
            //AddYear(2019);
            //AddQuarter(2019, 1);
            //AddQuarter(2019, 2);
            //AddQuarter(2019, 3);
            //AddQuarter(2019, 4);

            Console.WriteLine("DONE");
            Console.ReadLine();
        }

        static void AddMaster()
        {
            AddSeasonIfNotExists("Master", DateTimeOffset.MinValue, DateTimeOffset.MaxValue);
        }

        static void AddYear(int year)
        {
            var startDate = new DateTimeOffset(year, 1, 1, 0, 0, 0, TimeSpan.Zero);
            var endDate = startDate.AddYears(1);

            AddSeasonIfNotExists(year.ToString(), startDate, endDate);
        }

        static void AddQuarter(int year, int quarter)
        {
            if (quarter < 1 || quarter > 4)
            {
                throw new ArgumentOutOfRangeException(nameof(quarter), quarter, "Invalid quarter value. Must be between 1 and 4 inclusive.");
            }

            var startMonth = (quarter - 1) * 3 + 1;
            var startDate = new DateTimeOffset(year, startMonth, 1, 0, 0, 0, TimeSpan.Zero);
            var endDate = startDate.AddMonths(3);

            AddSeasonIfNotExists($"{year} Q{quarter}", startDate, endDate);
        }

        static bool AddSeasonIfNotExists(string name, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            if (SeasonHandler.GetSeason(name) != null)
            {
                Console.WriteLine($"Season {name} already exists");
                return false;
            }

            Console.WriteLine($"Add {name} season from {startDate} to {endDate}");
            SeasonHandler.AddSeason(new Season
            {
                Name = name,
                StartDate = startDate,
                EndDate = endDate
            });

            return true;
        }
    }
}
