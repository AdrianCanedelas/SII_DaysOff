using SII_DaysOff.Areas.Identity.Data;
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

        public virtual ApplicationUser? CreatedByNavigation { get; set; } = null!;
        public virtual ApplicationUser? ModifiedByNavigation { get; set; } = null!;
        public virtual Reasons? Reason { get; set; } = null!;
        public virtual Statuses? Status { get; set; } = null!;
        public virtual ApplicationUser? User { get; set; } = null!;
    }
}
