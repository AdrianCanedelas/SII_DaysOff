using System;
using System.Collections.Generic;

namespace SII_DaysOff.Models
{
    public partial class AspNetUsers
    {
        public AspNetUsers()
        {
            AspNetUserClaims = new HashSet<AspNetUserClaims>();
            AspNetUserLogins = new HashSet<AspNetUserLogins>();
            AspNetUserTokens = new HashSet<AspNetUserTokens>();
            RequestsIdAdminNavigation = new HashSet<Requests>();
            RequestsIdUserNavigation = new HashSet<Requests>();
            Role = new HashSet<AspNetRoles>();
        }

        public int Id { get; set; }
        public string Profile { get; set; } = null!;
        public int AvailableDays { get; set; }
        public int AcquiredDays { get; set; }
        public int RemainingDays { get; set; }
        public string? UserName { get; set; }
        public string? NormalizedUserName { get; set; }
        public string? Email { get; set; }
        public string? NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string? PasswordHash { get; set; }
        public string? SecurityStamp { get; set; }
        public string? ConcurrencyStamp { get; set; }
        public string? PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }

        public virtual ICollection<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual ICollection<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual ICollection<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual ICollection<Requests> RequestsIdAdminNavigation { get; set; }
        public virtual ICollection<Requests> RequestsIdUserNavigation { get; set; }

        public virtual ICollection<AspNetRoles> Role { get; set; }
    }
}
