using static Elo.Lib.Rating;

namespace Elo.Lib
{
    public class Player
    {
        public string Name { get; private set; }
        public int Rating { get; private set; }

        public Player(string name, int rating = Settings.DefaultRating)
        {
            Name = name;
            Rating = rating;
        }

        public void WinsAgainst(Player opponent)
        {
            var myNewRating = CalculateNewRating(Rating, opponent.Rating, Result.Win);
            var opponentNewRating = CalculateNewRating(opponent.Rating, Rating, Result.Loss);

            Rating = myNewRating;
            opponent.Rating = opponentNewRating;
        }

        public override string ToString()
        {
            return $"{Name}: {Rating}";
        }
    }
}
