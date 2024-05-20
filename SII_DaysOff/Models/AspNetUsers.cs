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
            InverseCreatedByNavigation = new HashSet<AspNetUsers>();
            InverseManagerNavigation = new HashSet<AspNetUsers>();
            InverseModifiedByNavigation = new HashSet<AspNetUsers>();
            ReasonsCreatedByNavigation = new HashSet<Reasons>();
            ReasonsModifiedByNavigation = new HashSet<Reasons>();
            RequestsCreatedByNavigation = new HashSet<Requests>();
            RequestsModifiedByNavigation = new HashSet<Requests>();
            RolesCreatedByNavigation = new HashSet<Roles>();
            RolesModifiedByNavigation = new HashSet<Roles>();
            UserVacationDaysCreatedByNavigation = new HashSet<UserVacationDays>();
            UserVacationDaysModifiedByNavigation = new HashSet<UserVacationDays>();
            VacationDaysCreatedByNavigation = new HashSet<VacationDays>();
            VacationDaysModifiedByNavigation = new HashSet<VacationDays>();
            Role = new HashSet<AspNetRoles>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public DateTime RegisterDate { get; set; }
        public bool IsActive { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public Guid ModifiedBy { get; set; }
        public DateTime ModificationDate { get; set; }
        public Guid Manager { get; set; }
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

        public virtual AspNetUsers CreatedByNavigation { get; set; } = null!;
        public virtual Roles Manager1 { get; set; } = null!;
        public virtual AspNetUsers ManagerNavigation { get; set; } = null!;
        public virtual AspNetUsers ModifiedByNavigation { get; set; } = null!;
        public virtual Requests? RequestsRequest { get; set; }
        public virtual UserVacationDays? UserVacationDaysUser { get; set; }
        public virtual ICollection<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual ICollection<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual ICollection<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual ICollection<AspNetUsers> InverseCreatedByNavigation { get; set; }
        public virtual ICollection<AspNetUsers> InverseManagerNavigation { get; set; }
        public virtual ICollection<AspNetUsers> InverseModifiedByNavigation { get; set; }
        public virtual ICollection<Reasons> ReasonsCreatedByNavigation { get; set; }
        public virtual ICollection<Reasons> ReasonsModifiedByNavigation { get; set; }
        public virtual ICollection<Requests> RequestsCreatedByNavigation { get; set; }
        public virtual ICollection<Requests> RequestsModifiedByNavigation { get; set; }
        public virtual ICollection<Roles> RolesCreatedByNavigation { get; set; }
        public virtual ICollection<Roles> RolesModifiedByNavigation { get; set; }
        public virtual ICollection<UserVacationDays> UserVacationDaysCreatedByNavigation { get; set; }
        public virtual ICollection<UserVacationDays> UserVacationDaysModifiedByNavigation { get; set; }
        public virtual ICollection<VacationDays> VacationDaysCreatedByNavigation { get; set; }
        public virtual ICollection<VacationDays> VacationDaysModifiedByNavigation { get; set; }

        public virtual ICollection<AspNetRoles> Role { get; set; }
    }
}
