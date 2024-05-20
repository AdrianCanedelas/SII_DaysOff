﻿using System;
using System.Collections.Generic;

namespace SII_DaysOff.Models
{
    public partial class UserVacationDays
    {
        public Guid UserId { get; set; }
        public string Year { get; set; } = null!;
        public int AcquiredDays { get; set; }
        public int AdditionalDays { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public Guid ModifiedBy { get; set; }
        public DateTime ModificationDate { get; set; }
    }
}