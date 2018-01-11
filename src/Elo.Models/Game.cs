using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Elo.Models
{
    [Table("Games")]
    public class Game : ModelBase
    {
        [MinLength(2)]
        [MaxLength(2)]
        public virtual List<GameScore> Scores { get; set; } = new List<GameScore>();

        public GameScore WinningGameScore => Scores.FirstOrDefault(gs => gs.Win);
        public GameScore LosingGameScore => Scores.FirstOrDefault(gs => gs.Loss);
    }
}
