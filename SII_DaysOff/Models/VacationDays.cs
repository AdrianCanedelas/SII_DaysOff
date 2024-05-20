using System;
using System.Collections.Generic;

namespace SII_DaysOff.Models
{
    public partial class VacationDays
    {
        public VacationDays()
        {
            UserVacationDays = new HashSet<UserVacationDays>();
        }

        public string Year { get; set; } = null!;
        public int DayVacations { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public Guid ModifiedBy { get; set; }
        public DateTime ModificationDate { get; set; }

        public virtual AspNetUsers CreatedByNavigation { get; set; } = null!;
        public virtual AspNetUsers ModifiedByNavigation { get; set; } = null!;
        public virtual ICollection<UserVacationDays> UserVacationDays { get; set; }
    }
}
