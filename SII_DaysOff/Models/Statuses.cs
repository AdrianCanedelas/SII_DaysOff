using System;
using System.Collections.Generic;

namespace SII_DaysOff.Models
{
    public partial class Statuses
    {
        public Statuses()
        {
            Requests = new HashSet<Requests>();
        }

        public Guid StatusId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public Guid CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public Guid ModifiedBy { get; set; }
        public DateTime ModificationDate { get; set; }

        public virtual AspNetUsers CreatedByNavigation { get; set; } = null!;
        public virtual AspNetUsers ModifiedByNavigation { get; set; } = null!;
        public virtual ICollection<Requests> Requests { get; set; }
    }
}
