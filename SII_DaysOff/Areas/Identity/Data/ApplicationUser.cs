using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using NuGet.Packaging.Signing;

namespace SII_DaysOff.Areas.Identity.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    public string Profile {  get; set; }
    public int AvailableDays { get; set; }
    public int AcquiredDays { get; set; }
    public int RemainingDays { get; set; }
}

