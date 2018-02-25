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

        public static Player GetPlayerByName(string name, bool includeRatings = false, bool includeGameScores = false)
        {
            using (var db = new EloDbContext())
            {
                var query = db.Players.AsQueryable();

                if (includeRatings)
                {
                    query = query.Include(p => p.Ratings);
                }

                if (includeGameScores)
                {
                    query = query.Include(p => p.GameScores);
                }

                var player = query.FirstOrDefault(p => p.Name == name);

                SortRatings(player);
                SortGameScores(player);

                return player;
            }
        }

        public static List<Player> GetAllPlayers(bool includeRatings = false, bool includeGameScores = false)
        {
            using (var db = new EloDbContext())
            {
                var query = db.Players.AsQueryable();

                if (includeRatings)
                {
                    query = query.Include(p => p.Ratings);
                }

                if (includeGameScores)
                {
                    query = query.Include(p => p.GameScores);
                }

                var players = query.ToList();

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

        public static void UpdatePlayers(IEnumerable<Player> players)
        {
            using (var db = new EloDbContext())
            {
                db.Players.UpdateRange(players);
                db.SaveChanges();
            }
        }

        public static void UpdatePlayers(params Player[] players) => UpdatePlayers(players.AsEnumerable());

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
