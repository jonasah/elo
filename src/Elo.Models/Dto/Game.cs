using System;
using System.Collections.Generic;
using System.Text;

namespace Elo.Models.Dto
{
    public class Game
    {
        public int Id { get; set; }
        public string Winner { get; set; }
        public string Loser { get; set; }
        public string Date { get; set; }
    }
}
