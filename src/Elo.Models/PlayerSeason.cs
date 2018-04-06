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
        public int CurrentPlayerRatingId { get; set; }
        public virtual PlayerRating CurrentPlayerRating { get; set; }

        public virtual List<PlayerSeasonRating> Ratings { get; set; }

        public Lib.Player ToEloLibPlayer()
        {
            return new Lib.Player(Player?.Name, CurrentPlayerRating.Rating);
        }
    }
}
