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
                return db.Players
                    .Include(p => p.Ratings)
                    .Include(p => p.GameScores)
                    .FirstOrDefault(p => p.Name == name);
            }
        }

        public static List<Player> GetAllPlayers()
        {
            using (var db = new EloDbContext())
            {
                return db.Players
                    .Include(p => p.Ratings)
                    .Include(p => p.GameScores)
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
    }
}
