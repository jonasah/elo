using static Elo.Lib.Rating;

namespace Elo.Lib
{
    public class Player
    {
        public string Name { get; private set; }
        public double Rating { get; private set; }

        public Player(string name, double rating = Settings.DefaultRating)
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

        public double ExpectedScore(Player opponent)
        {
            return GetExpectedScore(Rating, opponent.Rating);
        }

        public override string ToString()
        {
            return $"{Name}: {Rating}";
        }
    }
}
