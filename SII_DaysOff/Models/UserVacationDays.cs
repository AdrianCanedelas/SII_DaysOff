using SII_DaysOff.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SII_DaysOff.Models
{
    public partial class UserVacationDays
    {
        [Key]
		[ConcurrencyCheck]
		public Guid UserId { get; set; }
		[ConcurrencyCheck]
		public string Year { get; set; } = null!;
        public int AcquiredDays { get; set; }
        public int AdditionalDays { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public Guid ModifiedBy { get; set; }
        public DateTime ModificationDate { get; set; }

        public virtual ApplicationUser? CreatedByNavigation { get; set; } = null!;
        public virtual ApplicationUser? ModifiedByNavigation { get; set; } = null!;
        public virtual VacationDays? YearNavigation { get; set; } = null!;
        public virtual ApplicationUser? User {  get; set; } = null!;
    }
}
