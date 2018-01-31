using System;

namespace Elo.Lib
{
    public enum Result
    {
        Win,
        Loss
    }

    public static class Rating
    {
        public static int CalculateNewRating(int oldRating, int opponentRating, Result result)
        {
            var expected = GetExpectedScore(oldRating, opponentRating);
            var actual = result == Result.Win ? 1.0 : 0.0;
            var newRating = oldRating + Settings.KFactor * (actual - expected);
            return (int)(newRating + 0.5); // round to nearest integer
        }

        public static double GetExpectedScore(int myRating, int opponentRating)
        {
            return 1.0 / (1.0 + Math.Pow(10.0, (opponentRating - myRating) / 400.0));
        }
    }
}
