using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Elo.Models
{
    [Table("PlayerSeasons")]
    public class PlayerSeason : ModelBase
    {
        [Required]
        public int PlayerId { get; set; }
        public virtual Player Player { get; set; }

        [Required]
        public int SeasonId { get; set; }
        public virtual Season Season { get; set; }

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

        public virtual List<PlayerRating> Ratings { get; set; } = new List<PlayerRating>();

        public Lib.Player ToEloLibPlayer()
        {
            return new Lib.Player(Player?.Name, CurrentRating);
        }

        public PlayerRating CreatePlayerRating()
        {
            return new PlayerRating
            {
                PlayerSeasonId = Id,
                Rating = CurrentRating,
                Wins = Wins,
                Losses = Losses,
                CurrentStreak = CurrentStreak
            };
        }
    }
}
