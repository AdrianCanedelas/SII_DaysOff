using System;
using System.Collections.Generic;

namespace SII_DaysOff.Models
{
    public partial class VacationDays
    {
        public string Year { get; set; } = null!;
        public int DayVacations { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public Guid ModifiedBy { get; set; }
        public DateTime ModificationDate { get; set; }
    }
}
