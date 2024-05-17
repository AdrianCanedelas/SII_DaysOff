using System;
using System.Collections.Generic;

namespace SII_DaysOff.Models
{
    public partial class AspNetRoles
    {
        public AspNetRoles()
        {
            AspNetRoleClaims = new HashSet<AspNetRoleClaims>();
            User = new HashSet<AspNetUsers>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? NormalizedName { get; set; }
        public string? ConcurrencyStamp { get; set; }

        public virtual ICollection<AspNetRoleClaims> AspNetRoleClaims { get; set; }

        public virtual ICollection<AspNetUsers> User { get; set; }
    }
}
