using System;
using System.Collections.Generic;

namespace SII_DaysOff.Models
{
    public partial class AspNetUserTokens
    {
        public int UserId { get; set; }
        public string LoginProvider { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Value { get; set; }

        public virtual AspNetUsers User { get; set; } = null!;
    }
}
