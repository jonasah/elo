using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Elo.Models
{
    [Table("Players")]
    public class Player : ModelBase
    {
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Name { get; set; }

        public virtual List<GameScore> GameScores { get; set; } = new List<GameScore>();

        public virtual List<PlayerRating> Ratings { get; set; } = new List<PlayerRating>();

        [Required]
        public double CurrentRating { get; set; } = Lib.Settings.DefaultRating;
        [Required]
        public int Wins { get; set; }
        [Required]
        public int Losses { get; set; }
        [Required]
        public int CurrentStreak { get; set; }

        public int GamesPlayed => Wins + Losses;
        public double Pct => (double)Wins / GamesPlayed;

        public Lib.Player ToEloLibPlayer()
        {
            return new Lib.Player(Name, CurrentRating);
        }

        public PlayerRating CreatePlayerRating()
        {
            return new PlayerRating
            {
                PlayerId = Id,
                Rating = CurrentRating,
                Wins = Wins,
                Losses = Losses,
                CurrentStreak = CurrentStreak
            };
        }
    }
}
