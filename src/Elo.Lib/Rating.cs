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
        public static double CalculateNewRating(double oldRating, double opponentRating, Result result)
        {
            var expected = GetExpectedScore(oldRating, opponentRating);
            var actual = result == Result.Win ? 1.0 : 0.0;
            var newRating = oldRating + Settings.KFactor * (actual - expected);
            return newRating; // round to nearest integer
        }

        public static double GetExpectedScore(double myRating, double opponentRating)
        {
            return 1.0 / (1.0 + Math.Pow(10.0, (opponentRating - myRating) / 400.0));
        }
    }
}
