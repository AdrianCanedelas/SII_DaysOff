using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SII_DaysOff.Areas.Identity.Data;
using SII_DaysOff.Models;
using System.Reflection.Emit;

namespace SII_DaysOff.Areas.Identity.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);

        /*builder.Entity<ApplicationUser>(entity =>
        {
            entity.HasIndex(e => e.RoleID);

            entity.HasOne(d => d.RoleIDUser)
                .WithMany(p => p.AspNetRoleClaims)
                .HasForeignKey(d => d.RoleId);
        });*/

        builder.ApplyConfiguration(new ApplicationUserEntityConfiguration());
    }

    private class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            /*builder.Property(u => u.Profile).HasMaxLength(50);
            builder.Property(u => u.AvailableDays);
            builder.Property(u => u.AcquiredDays);
            builder.Property(u => u.RemainingDays);*/
            //builder.Property(u => u.RoleID).IsRequired();
            builder.Property(u => u.Name).HasMaxLength(100).IsRequired();
            builder.Property(u => u.Surname).HasMaxLength(100).IsRequired();
            builder.Property(u => u.RegisterDate).IsRequired();
            builder.Property(u => u.IsActive).IsRequired();
            builder.Property(u => u.CreatedBy).IsRequired(false);
            builder.Property(u => u.CreationDate).IsRequired();
            builder.Property(u => u.ModifiedBy).IsRequired(false);
            builder.Property(u => u.ModificationDate).IsRequired();
            builder.Property(u => u.Manager).IsRequired(false);

            //builder.HasOne(u => u.RoleIDUser).WithMany().HasForeignKey(u => u.RoleID);
            builder.HasOne(u => u.CreatedByUser).WithMany().HasForeignKey(u => u.CreatedBy);
            builder.HasOne(u => u.ModifiedByUser).WithMany().HasForeignKey(u => u.ModifiedBy);
            builder.HasOne(u => u.ManagerUser).WithMany().HasForeignKey(u => u.Manager);
        }
    }
}
