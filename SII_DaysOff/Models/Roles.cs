using Microsoft.AspNetCore.Identity;
using SII_DaysOff.Areas.Identity.Data;
using System;
using System.Collections.Generic;

namespace SII_DaysOff.Models
{
    public partial class Roles : IdentityRole<Guid>
    {
        public Roles()
        {
            AspNetUsers = new HashSet<ApplicationUser>();
        }

        public string Description { get; set; } = null!;
        public Guid? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModificationDate { get; set; }

        public virtual ApplicationUser? CreatedByNavigation { get; set; }
        public virtual ApplicationUser? ModifiedByNavigation { get; set; }
        public virtual ICollection<ApplicationUser>? AspNetUsers { get; set; }
    }
}
