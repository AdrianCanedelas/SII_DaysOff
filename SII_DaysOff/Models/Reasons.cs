using SII_DaysOff.Areas.Identity.Data;
using System;
using System.Collections.Generic;

namespace SII_DaysOff.Models
{
    public partial class Reasons
    {
        public Reasons()
        {
            Requests = new HashSet<Requests>();
        }

        public Guid ReasonId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public Guid CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public Guid ModifiedBy { get; set; }
        public DateTime ModificationDate { get; set; }

        public virtual ApplicationUser CreatedByNavigation { get; set; } = null!;
        public virtual ApplicationUser ModifiedByNavigation { get; set; } = null!;
        public virtual ICollection<Requests> Requests { get; set; }
    }
}
