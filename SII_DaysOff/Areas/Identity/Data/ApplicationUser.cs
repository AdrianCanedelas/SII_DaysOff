using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using NuGet.Packaging.Signing;
using SII_DaysOff.Models;

namespace SII_DaysOff.Areas.Identity.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser<Guid>
{
    /*public Guid? RoleID { get; set; }
    public Roles? RoleIDUser { get; set; }*/
    public string Name { get; set; }
    public string Surname { get; set; }
    public DateTime RegisterDate { get; set; }
    public bool IsActive { get; set; }
    public Guid? CreatedBy { get; set; }
    public ApplicationUser? CreatedByUser { get; set; }
    public DateTime CreationDate { get; set; }
    public Guid? ModifiedBy { get; set; }
    public ApplicationUser? ModifiedByUser { get; set; }
    public DateTime ModificationDate { get; set; }
    public Guid? Manager { get; set; }
    public ApplicationUser? ManagerUser { get; set; }
}

