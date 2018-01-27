using Elo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Elo.DbHandler
{
    public static class GameHandler
    {
        public static Game AddGame(Game game)
        {
            using (var db = new EloDbContext())
            {
                db.Games.Add(game);
                db.SaveChanges();
            }

            return game;
        }

        public static List<Game> GetGames(int page, int pageSize)
        {
            using (var db = new EloDbContext())
            {
                return db.Games
                    .Include(g => g.Scores)
                        .ThenInclude(gs => gs.Player)
                    .OrderByDescending(g => g.Created)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            }
        }

        public static List<Game> GetGamesByPlayer(string player)
        {
            using (var db = new EloDbContext())
            {
                return db.GameScores
                    .Include(gs => gs.Player)
                    .Include(gs => gs.Game)
                        .ThenInclude(g => g.Scores)
                            .ThenInclude(gs => gs.Player)
                    .ToList()
                    .Where(gs => gs.Player.Name == player)
                    .Select(gs => gs.Game)
                    .OrderByDescending(g => g.Created)
                    .ToList();
            }
        }

        public static List<Game> GetGamesByPlayer(string player, int page, int pageSize)
        {
            using (var db = new EloDbContext())
            {
                return db.GameScores
                    .Include(gs => gs.Player)
                    .Include(gs => gs.Game)
                        .ThenInclude(g => g.Scores)
                            .ThenInclude(gs => gs.Player)
                    .ToList()
                    .Where(gs => gs.Player.Name == player)
                    .Select(gs => gs.Game)
                    .OrderByDescending(g => g.Created)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            }
        }

        public static Game GetGame(int id)
        {
            using (var db = new EloDbContext())
            {
                return db.Games
                    .Include(g => g.Scores)
                        .ThenInclude(gs => gs.Player)
                    .FirstOrDefault(g => g.Id == id);
            }
        }

        public static List<Game> GetGamesAfter(DateTime timestamp)
        {
            using (var db = new EloDbContext())
            {
                return db.Games
                    .Where(g => g.Created > timestamp)
                    .Include(g => g.Scores)
                        .ThenInclude(gs => gs.Player)
                    .ToList();
            }
        }

        public static void DeleteGame(Game game)
        {
            using (var db = new EloDbContext())
            {
                db.Games.Remove(game);
                db.SaveChanges();
            }
        }
    }
}
