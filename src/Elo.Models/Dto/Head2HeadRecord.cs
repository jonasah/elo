using System;
using System.Collections.Generic;
using System.Text;

namespace Elo.Models.Dto
{
    public class Head2HeadRecord
    {
        public string Opponent { get; set; }
        public int GamesPlayed => Wins + Losses;
        public int Wins { get; set; }
        public int Losses { get; set; }
        public double Pct => (double)Wins / GamesPlayed;
        public double RatingChange { get; set; }
    }
}
