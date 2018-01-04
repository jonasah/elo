using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Elo.Models
{
    [Table("Games")]
    public class Game : ModelBase
    {
        [MinLength(2)]
        [MaxLength(2)]
        public virtual List<GameScore> Scores { get; set; } = new List<GameScore>();
    }
}
