using Elo.DbHandler;
using Elo.Models;
using System;

namespace Elo.AddSeasons
{
    class Program
    {
        static void Main(string[] args)
        {
            AddMaster();
            AddYear(2018, addQuarters: true);
            AddYear(2019, addQuarters: true);

            Console.WriteLine("DONE");
            Console.ReadLine();
        }

        static void AddMaster()
        {
            AddSeasonIfNotExists("Master", DateTimeOffset.MinValue, DateTimeOffset.MaxValue);
        }

        static void AddYear(int year, bool addQuarters = false)
        {
            var startDate = new DateTimeOffset(year, 1, 1, 0, 0, 0, TimeSpan.Zero);
            var endDate = startDate.AddYears(1);

            AddSeasonIfNotExists(year.ToString(), startDate, endDate);

            if (addQuarters)
            {
                for (var quarter = 1; quarter <= 4; ++quarter)
                {
                    var quarterStartMonth = (quarter - 1) * 3 + 1;
                    var quarterStartDate = new DateTimeOffset(year, quarterStartMonth, 1, 0, 0, 0, TimeSpan.Zero);
                    var quarterEndDate = quarterStartDate.AddMonths(3);

                    AddSeasonIfNotExists($"{year} Q{quarter}", quarterStartDate, quarterEndDate);
                }
            }
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
