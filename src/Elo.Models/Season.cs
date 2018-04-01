using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Elo.Models
{
    [Table("Seasons")]
    public class Season : ModelBase
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public DateTimeOffset StartDate { get; set; }
        [Required]
        public DateTimeOffset EndDate { get; set; }

        public bool IsActive(DateTimeOffset dateTime)
        {
            return StartDate <= dateTime && dateTime < EndDate;
        }

        public bool HasStarted(DateTimeOffset dateTime)
        {
            return dateTime >= StartDate;
        }

        public bool HasEnded(DateTimeOffset dateTime)
        {
            return dateTime >= EndDate;
        }
    }
}
