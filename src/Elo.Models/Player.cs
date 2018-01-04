using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

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

        [NotMapped]
        public int CurrentRating
        {
            get => Ratings.LastOrDefault().Rating;
            set => Ratings.Add(new PlayerRating
            {
                Rating = value
            });
        }

        public Lib.Player ToEloLibPlayer()
        {
            return new Lib.Player(Name, CurrentRating);
        }
    }
}
