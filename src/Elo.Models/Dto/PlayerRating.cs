namespace Elo.Models.Dto
{
    public class PlayerRating
    {
        public int Id { get; set; }
        public int Rank { get; set; }
        public string Player { get; set; }
        public int Rating { get; set; }
        public int GamesPlayed => Wins + Losses;
        public int Wins { get; set; }
        public int Losses { get; set; }
        public double Pct => (double)Wins / GamesPlayed;
        public int Streak { get; set; }
    }
}
