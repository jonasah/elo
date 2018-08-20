using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public List<PlayerRating> PlayerRatings { get; set; }

        public Lib.Player ToEloLibPlayer()
        {
            return new Lib.Player(Player?.Name, Rating);
        }

        public PlayerRating CreatePlayerRating(int? gameId = null)
        {
            return new PlayerRating
            {
                PlayerSeasonId = Id,
                GameId = gameId,
                Rating = Rating,
                RatingChange = RatingChange,
                Wins = Wins,
                Losses = Losses,
                CurrentStreak = CurrentStreak
            };
        }
    }
}
