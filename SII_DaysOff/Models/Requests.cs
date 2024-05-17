using System;
using System.Collections.Generic;

namespace SII_DaysOff.Models
{
    public partial class Requests
    {
        public int IdRequest { get; set; }
        public int IdUser { get; set; }
        public int IdAdmin { get; set; }
        public int IdReason { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalDays { get; set; }
        public byte HalfDayStart { get; set; }
        public byte HalfDayEnd { get; set; }
        public string Status { get; set; } = null!;

        public virtual AspNetUsers? IdAdminNavigation { get; set; } = null!;
        public virtual Reasons? IdReasonNavigation { get; set; } = null!;
        public virtual AspNetUsers? IdUserNavigation { get; set; } = null!;
    }
}
