using System;
using System.Collections.Generic;

namespace SII_DaysOff.Models
{
    public partial class Requests
    {
        public Guid RequestId { get; set; }
        public Guid UserId { get; set; }
        public Guid ReasonId { get; set; }
        public Guid StatusId { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool HalfDayStart { get; set; }
        public bool HalfDayEnd { get; set; }
        public string Comments { get; set; } = null!;
        public Guid CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public Guid ModifiedBy { get; set; }
        public DateTime ModificationDate { get; set; }

        public virtual AspNetUsers? CreatedByNavigation { get; set; } = null!;
        public virtual AspNetUsers? ModifiedByNavigation { get; set; } = null!;
        public virtual Reasons Reason { get; set; } = null!;
        public virtual Statuses Status { get; set; } = null!;
        public virtual AspNetUsers User { get; set; } = null!;
    }
}
