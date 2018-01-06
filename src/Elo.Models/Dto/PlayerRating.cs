namespace Elo.Models.Dto
{
    public class PlayerRating
    {
        public int Id { get; set; }
        public int Rank { get; set; }
        public string Player { get; set; }
        public int Rating { get; set; }
        public int GamesPlayed { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public double Pct { get; set; }
    }
}
