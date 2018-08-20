using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Elo.Models
{
    [Table("PlayerRatings")]
    public class PlayerRating : ModelBase
    {
        public int PlayerSeasonId { get; set; }
        public virtual PlayerSeason PlayerSeason { get; set; }

        public int? GameId { get; set; }
        public virtual Game Game { get; set; }

        [Required]
        public double Rating { get; set; } = Lib.Settings.DefaultRating;
        [Required]
        public double RatingChange { get; set; }
        [Required]
        public int Wins { get; set; }
        [Required]
        public int Losses { get; set; }
        [Required]
        public int CurrentStreak { get; set; }

        public int GamesPlayed => Wins + Losses;
        public double Pct => (double)Wins / GamesPlayed;
    }
}
