using System;
using System.Collections.Generic;

namespace SII_DaysOff.Models
{
    public partial class Roles
    {
        public Guid RoleId { get; set; }
        public string Description { get; set; } = null!;
        public Guid CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public Guid ModifiedBy { get; set; }
        public DateTime ModificationDate { get; set; }
    }
}
