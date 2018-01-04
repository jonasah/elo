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
            var expected = 1.0 / (1.0 + Math.Pow(10.0, (opponentRating - oldRating) / 400.0));
            var actual = result == Result.Win ? 1.0 : 0.0;
            return (int)(oldRating + Settings.KFactor * (actual - expected));
        }
    }
}
