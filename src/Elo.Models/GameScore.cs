using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Elo.Models
{
    [Table("GameScores")]
    public class GameScore : ModelBase
    {
        [Required]
        public int GameId { get; set; }
        public virtual Game Game { get; set; }

        [Required]
        public int PlayerId { get; set; }
        public virtual Player Player { get; set; }

        [Required]
        [Range(0.0, 1.0)]
        public double Score { get; set; }

        public bool Win => Score > 0.5;
        public bool Loss => Score < 0.5;
    }
}
