using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Elo.Models
{
    [Table("PlayerSeasonRatings")]
    public class PlayerSeasonRating : ModelBase
    {
        [Required]
        public int PlayerSeasonId { get; set; }
        public PlayerSeason PlayerSeason { get; set; }

        [Required]
        public int PlayerRatingId { get; set; }
        public PlayerRating PlayerRating { get; set; }
    }
}
