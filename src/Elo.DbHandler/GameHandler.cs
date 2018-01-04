using Elo.Models;

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
    }
}
