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

        public int IdReason { get; set; }
        public string ReasonName { get; set; } = null!;
        public int DaysAssigned { get; set; }

        public virtual ICollection<Requests> Requests { get; set; }
    }
}
