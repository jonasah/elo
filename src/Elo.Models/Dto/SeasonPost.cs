using System;
using System.Collections.Generic;
using System.Text;

namespace Elo.Models.Dto
{
    public class SeasonPost
    {
        public string Name { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
    }
}
