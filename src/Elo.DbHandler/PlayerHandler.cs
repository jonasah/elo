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
            using (var db = new EloDbContext())
            {
                db.Players.Add(player);
                db.SaveChanges();
            }

            return player;
        }

        public static Player GetPlayerById(int id)
        {
            using (var db = new EloDbContext())
            {
                return db.Players
                    .Include(p => p.Seasons)
                    .FirstOrDefault(p => p.Id == id);
            }
        }

        public static Player GetPlayerByName(string name)
        {
            using (var db = new EloDbContext())
            {
                return db.Players
                    .Include(p => p.Seasons)
                    .FirstOrDefault(p => p.Name == name);
            }
        }

        public static List<Player> GetAllPlayers()
        {
            using (var db = new EloDbContext())
            {
                return db.Players
                    .Include(p => p.Seasons)
                    .ToList();
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

        public static void UpdatePlayers(params Player[] players) =>
            UpdatePlayers(players.AsEnumerable());

        public static List<PlayerSeason> GetAllPlayerSeasons(int seasonId)
        {
            using (var db = new EloDbContext())
            {
                return db.PlayerSeasons
                    .Where(s => s.SeasonId == seasonId)
                    .Include(ps => ps.Player)
                    .ToList();
            }
        }

        public static List<PlayerSeason> GetAllPlayerSeasons(string seasonName)
        {
            var season = SeasonHandler.GetSeason(seasonName);

            if (season == null)
            {
                return new List<PlayerSeason>();
            }

            return GetAllPlayerSeasons(season.Id);
        }

        public static PlayerSeason AddPlayerSeason(PlayerSeason playerSeason)
        {
            using (var db = new EloDbContext())
            {
                db.PlayerSeasons.Add(playerSeason);
                db.SaveChanges();
            }

            return playerSeason;
        }

        public static void UpdatePlayerSeasons(IEnumerable<PlayerSeason> playerSeasons)
        {
            using (var db = new EloDbContext())
            {
                db.PlayerSeasons.UpdateRange(playerSeasons);
                db.SaveChanges();
            }
        }

        public static void UpdatePlayerSeasons(params PlayerSeason[] playerSeasons) =>
            UpdatePlayerSeasons(playerSeasons.AsEnumerable());

        public static void DeleteAllPlayerSeasons()
        {
            using (var db = new EloDbContext())
            {
                db.PlayerSeasons.RemoveRange(db.PlayerSeasons);
                db.SaveChanges();
            }
        }
    }
}
