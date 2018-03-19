using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Elo.Models
{
    [Table("PlayerRatings")]
    public class PlayerRating : ModelBase
    {
        [Required]
        public int PlayerId { get; set; }
        public virtual Player Player { get; set; }

        [Required]
        public double Rating { get; set; } = Lib.Settings.DefaultRating;

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
