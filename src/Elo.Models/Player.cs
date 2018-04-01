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

        public virtual List<PlayerSeason> Seasons { get; set; } = new List<PlayerSeason>();
    }
}
