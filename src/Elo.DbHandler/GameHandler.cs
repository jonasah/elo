using Elo.Models;
using Microsoft.EntityFrameworkCore;
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
                    .ToList();
            }
        }
    }
}
