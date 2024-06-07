﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SII_DaysOff.Models;

#nullable disable

namespace SII_DaysOff.Migrations
{
    [DbContext(typeof(DbContextBD))]
    partial class DbContextBDModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.29")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("SII_DaysOff.Areas.Identity.Data.ApplicationUser", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("Manager")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ModificationDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("ModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<DateTime>("RegisterDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy")
                        .IsUnique()
                        .HasFilter("[CreatedBy] IS NOT NULL");

                    b.HasIndex("Manager")
                        .IsUnique()
                        .HasFilter("[Manager] IS NOT NULL");

                    b.HasIndex("ModifiedBy")
                        .IsUnique()
                        .HasFilter("[ModifiedBy] IS NOT NULL");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("RoleId");

                    b.HasIndex(new[] { "NormalizedEmail" }, "EmailIndex");

                    b.HasIndex(new[] { "NormalizedUserName" }, "UserNameIndex")
                        .IsUnique()
                        .HasFilter("([NormalizedUserName] IS NOT NULL)");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("SII_DaysOff.Models.Reasons", b =>
                {
                    b.Property<Guid>("ReasonId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("ReasonID");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime>("ModificationDate")
                        .HasColumnType("datetime");

                    b.Property<Guid>("ModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.HasKey("ReasonId");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("ModifiedBy");

                    b.ToTable("Reasons", (string)null);
                });

            modelBuilder.Entity("SII_DaysOff.Models.Requests", b =>
                {
                    b.Property<Guid>("RequestId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("RequestID")
                        .HasDefaultValueSql("(newid())");

                    b.Property<string>("Comments")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime");

                    b.Property<bool>("HalfDayEnd")
                        .HasColumnType("bit");

                    b.Property<bool>("HalfDayStart")
                        .HasColumnType("bit");

                    b.Property<DateTime>("ModificationDate")
                        .HasColumnType("datetime");

                    b.Property<Guid>("ModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ReasonId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("ReasonID");

                    b.Property<DateTime>("RequestDate")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime");

                    b.Property<Guid>("StatusId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("StatusID");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("UserID");

                    b.HasKey("RequestId");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("ModifiedBy");

                    b.HasIndex("ReasonId");

                    b.HasIndex("StatusId");

                    b.HasIndex("UserId");

                    b.ToTable("Requests", (string)null);
                });

            modelBuilder.Entity("SII_DaysOff.Models.Roles", b =>
                {
                    b.Property<Guid>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("RoleID")
                        .HasDefaultValueSql("(newid())");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("CreationDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("ModificationDate")
                        .HasColumnType("datetime");

                    b.Property<Guid?>("ModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("RoleId");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("ModifiedBy");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("SII_DaysOff.Models.Statuses", b =>
                {
                    b.Property<Guid>("StatusId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("StatusID")
                        .HasDefaultValueSql("(newid())");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime>("ModificationDate")
                        .HasColumnType("datetime");

                    b.Property<Guid>("ModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.HasKey("StatusId");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("ModifiedBy");

                    b.ToTable("Statuses", (string)null);
                });

            modelBuilder.Entity("SII_DaysOff.Models.UserVacationDays", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("UserID");

                    b.Property<int>("AcquiredDays")
                        .HasColumnType("int");

                    b.Property<int>("AdditionalDays")
                        .HasColumnType("int");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("ModificationDate")
                        .HasColumnType("datetime");

                    b.Property<Guid>("ModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Year")
                        .IsRequired()
                        .HasMaxLength(4)
                        .IsUnicode(false)
                        .HasColumnType("varchar(4)");

                    b.HasKey("UserId");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("ModifiedBy");

                    b.HasIndex("Year");

                    b.ToTable("UserVacationDays", (string)null);
                });

            modelBuilder.Entity("SII_DaysOff.Models.VacationDays", b =>
                {
                    b.Property<string>("Year")
                        .HasMaxLength(4)
                        .IsUnicode(false)
                        .HasColumnType("varchar(4)");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime");

                    b.Property<int>("DayVacations")
                        .HasColumnType("int");

                    b.Property<DateTime>("ModificationDate")
                        .HasColumnType("datetime");

                    b.Property<Guid>("ModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Year");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("ModifiedBy");

                    b.ToTable("VacationDays", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("SII_DaysOff.Models.Roles", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("SII_DaysOff.Areas.Identity.Data.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("SII_DaysOff.Areas.Identity.Data.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("SII_DaysOff.Models.Roles", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SII_DaysOff.Areas.Identity.Data.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("SII_DaysOff.Areas.Identity.Data.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SII_DaysOff.Areas.Identity.Data.ApplicationUser", b =>
                {
                    b.HasOne("SII_DaysOff.Areas.Identity.Data.ApplicationUser", "CreatedByUser")
                        .WithOne()
                        .HasForeignKey("SII_DaysOff.Areas.Identity.Data.ApplicationUser", "CreatedBy")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("SII_DaysOff.Areas.Identity.Data.ApplicationUser", "ManagerUser")
                        .WithOne()
                        .HasForeignKey("SII_DaysOff.Areas.Identity.Data.ApplicationUser", "Manager")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("SII_DaysOff.Areas.Identity.Data.ApplicationUser", "ModifiedByUser")
                        .WithOne()
                        .HasForeignKey("SII_DaysOff.Areas.Identity.Data.ApplicationUser", "ModifiedBy")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("SII_DaysOff.Models.Roles", "RoleIdUser")
                        .WithMany("AspNetUsers")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("FK_Users_RoleId");

                    b.Navigation("CreatedByUser");

                    b.Navigation("ManagerUser");

                    b.Navigation("ModifiedByUser");

                    b.Navigation("RoleIdUser");
                });

            modelBuilder.Entity("SII_DaysOff.Models.Reasons", b =>
                {
                    b.HasOne("SII_DaysOff.Areas.Identity.Data.ApplicationUser", "CreatedByNavigation")
                        .WithMany()
                        .HasForeignKey("CreatedBy")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("FK_Reasons_AspNetUsers");

                    b.HasOne("SII_DaysOff.Areas.Identity.Data.ApplicationUser", "ModifiedByNavigation")
                        .WithMany()
                        .HasForeignKey("ModifiedBy")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("FK_Reasons_AspNetUsers1");

                    b.Navigation("CreatedByNavigation");

                    b.Navigation("ModifiedByNavigation");
                });

            modelBuilder.Entity("SII_DaysOff.Models.Requests", b =>
                {
                    b.HasOne("SII_DaysOff.Areas.Identity.Data.ApplicationUser", "CreatedByNavigation")
                        .WithMany()
                        .HasForeignKey("CreatedBy")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("FK_Requests_AspNetUsers1");

                    b.HasOne("SII_DaysOff.Areas.Identity.Data.ApplicationUser", "ModifiedByNavigation")
                        .WithMany()
                        .HasForeignKey("ModifiedBy")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("FK_Requests_AspNetUsers2");

                    b.HasOne("SII_DaysOff.Models.Reasons", "Reason")
                        .WithMany("Requests")
                        .HasForeignKey("ReasonId")
                        .IsRequired()
                        .HasConstraintName("FK_Requests_Reasons");

                    b.HasOne("SII_DaysOff.Models.Statuses", "Status")
                        .WithMany("Requests")
                        .HasForeignKey("StatusId")
                        .IsRequired()
                        .HasConstraintName("FK_Requests_Statuses");

                    b.HasOne("SII_DaysOff.Areas.Identity.Data.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("FK_Requests_AspNetUsers");

                    b.Navigation("CreatedByNavigation");

                    b.Navigation("ModifiedByNavigation");

                    b.Navigation("Reason");

                    b.Navigation("Status");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SII_DaysOff.Models.Roles", b =>
                {
                    b.HasOne("SII_DaysOff.Areas.Identity.Data.ApplicationUser", "CreatedByNavigation")
                        .WithMany()
                        .HasForeignKey("CreatedBy")
                        .HasConstraintName("FK_Roles_AspNetUsers");

                    b.HasOne("SII_DaysOff.Areas.Identity.Data.ApplicationUser", "ModifiedByNavigation")
                        .WithMany()
                        .HasForeignKey("ModifiedBy")
                        .HasConstraintName("FK_Roles_AspNetUsers1");

                    b.Navigation("CreatedByNavigation");

                    b.Navigation("ModifiedByNavigation");
                });

            modelBuilder.Entity("SII_DaysOff.Models.Statuses", b =>
                {
                    b.HasOne("SII_DaysOff.Areas.Identity.Data.ApplicationUser", "CreatedByNavigation")
                        .WithMany()
                        .HasForeignKey("CreatedBy")
                        .IsRequired()
                        .HasConstraintName("FK_Statuses_AspNetUsers");

                    b.HasOne("SII_DaysOff.Areas.Identity.Data.ApplicationUser", "ModifiedByNavigation")
                        .WithMany()
                        .HasForeignKey("ModifiedBy")
                        .IsRequired()
                        .HasConstraintName("FK_Statuses_AspNetUsers1");

                    b.Navigation("CreatedByNavigation");

                    b.Navigation("ModifiedByNavigation");
                });

            modelBuilder.Entity("SII_DaysOff.Models.UserVacationDays", b =>
                {
                    b.HasOne("SII_DaysOff.Areas.Identity.Data.ApplicationUser", "CreatedByNavigation")
                        .WithMany()
                        .HasForeignKey("CreatedBy")
                        .IsRequired()
                        .HasConstraintName("FK_UserVacationDays_AspNetUsers");

                    b.HasOne("SII_DaysOff.Areas.Identity.Data.ApplicationUser", "ModifiedByNavigation")
                        .WithMany()
                        .HasForeignKey("ModifiedBy")
                        .IsRequired()
                        .HasConstraintName("FK_UserVacationDays_AspNetUsers1");

                    b.HasOne("SII_DaysOff.Areas.Identity.Data.ApplicationUser", "User")
                        .WithOne("UserVacationDays")
                        .HasForeignKey("SII_DaysOff.Models.UserVacationDays", "UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("FK_UserVacationDays_userId");

                    b.HasOne("SII_DaysOff.Models.VacationDays", "YearNavigation")
                        .WithMany("UserVacationDays")
                        .HasForeignKey("Year")
                        .IsRequired()
                        .HasConstraintName("FK_UserVacationDays_VacationDays");

                    b.Navigation("CreatedByNavigation");

                    b.Navigation("ModifiedByNavigation");

                    b.Navigation("User");

                    b.Navigation("YearNavigation");
                });

            modelBuilder.Entity("SII_DaysOff.Models.VacationDays", b =>
                {
                    b.HasOne("SII_DaysOff.Areas.Identity.Data.ApplicationUser", "CreatedByNavigation")
                        .WithMany()
                        .HasForeignKey("CreatedBy")
                        .IsRequired()
                        .HasConstraintName("FK_VacationDays_AspNetUsers");

                    b.HasOne("SII_DaysOff.Areas.Identity.Data.ApplicationUser", "ModifiedByNavigation")
                        .WithMany()
                        .HasForeignKey("ModifiedBy")
                        .IsRequired()
                        .HasConstraintName("FK_VacationDays_AspNetUsers1");

                    b.Navigation("CreatedByNavigation");

                    b.Navigation("ModifiedByNavigation");
                });

            modelBuilder.Entity("SII_DaysOff.Areas.Identity.Data.ApplicationUser", b =>
                {
                    b.Navigation("UserVacationDays")
                        .IsRequired();
                });

            modelBuilder.Entity("SII_DaysOff.Models.Reasons", b =>
                {
                    b.Navigation("Requests");
                });

            modelBuilder.Entity("SII_DaysOff.Models.Roles", b =>
                {
                    b.Navigation("AspNetUsers");
                });

            modelBuilder.Entity("SII_DaysOff.Models.Statuses", b =>
                {
                    b.Navigation("Requests");
                });

            modelBuilder.Entity("SII_DaysOff.Models.VacationDays", b =>
                {
                    b.Navigation("UserVacationDays");
                });
#pragma warning restore 612, 618
        }
    }
}
