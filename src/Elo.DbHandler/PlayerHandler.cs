using Elo.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Elo.DbHandler
{
    public static class PlayerHandler
    {
        public static Player AddPlayer(Player player)
        {
            player.Ratings.Clear();
            player.Ratings.Add(new PlayerRating());

            using (var db = new EloDbContext())
            {
                db.Players.Add(player);
                db.SaveChanges();
            }

            return player;
        }

        public static Player GetPlayerByName(string name)
        {
            using (var db = new EloDbContext())
            {
                var player = db.Players
                    .Include(p => p.Ratings)
                    .Include(p => p.GameScores)
                    .FirstOrDefault(p => p.Name == name);

                SortRatings(player);
                SortGameScores(player);

                return player;
            }
        }

        public static List<Player> GetAllPlayers()
        {
            using (var db = new EloDbContext())
            {
                var players = db.Players
                    .Include(p => p.Ratings)
                    .Include(p => p.GameScores)
                    .ToList();

                players.ForEach(p => SortRatings(p));
                players.ForEach(p => SortGameScores(p));

                return players;
            }
        }

        public static List<string> GetAllPlayerNames()
        {
            using (var db = new EloDbContext())
            {
                return db.Players
                    .Select(p => p.Name)
                    .OrderBy(p => p)
                    .ToList();
            }
        }

        public static Player UpdatePlayer(Player player)
        {
            using (var db = new EloDbContext())
            {
                db.Players.Update(player);
                db.SaveChanges();
            }

            return player;
        }

        private static void SortRatings(Player player)
        {
            player?.Ratings.Sort((r1, r2) => r1.Created.CompareTo(r2.Created));
        }

        private static void SortGameScores(Player player)
        {
            player?.GameScores.Sort((gs1, gs2) => gs1.Created.CompareTo(gs2.Created));
        }
    }
}
