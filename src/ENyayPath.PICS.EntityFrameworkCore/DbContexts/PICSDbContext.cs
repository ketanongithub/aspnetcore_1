using ENyayPath.PICS.Core.Auditing;
using ENyayPath.PICS.Core.Authorization.Roles;
using ENyayPath.PICS.Core.Authorization.Users;
using ENyayPath.PICS.Core.BackgroundJob;
using ENyayPath.PICS.Core.Editions;
using ENyayPath.PICS.Core.Features;
using ENyayPath.PICS.Core.Localization;
using ENyayPath.PICS.Core.MultiTenancy;
using ENyayPath.PICS.Core.Notifications;
using ENyayPath.PICS.Core.OrganizationUnit;
using ENyayPath.PICS.Core.Settings;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;

namespace ENyayPath.PICS.EntityFrameworkCore.DbContexts
{
    public class PICSDbContext : IdentityDbContext<User, Role, long>
    {
        public PICSDbContext(DbContextOptions<PICSDbContext> options) : base(options) { }

        // Sys kernel
        public DbSet<Tenant> Tenants { get; set; } = default!;
        public DbSet<Edition> Editions { get; set; } = default!;
        public DbSet<Feature> Features { get; set; } = default!;
        public DbSet<FeatureValue> FeatureValues { get; set; } = default!;
        public DbSet<AuditLog> AuditLogs { get; set; } = default!;
        public DbSet<Setting> Settings { get; set; } = default!;
        public DbSet<BackgroundJobInfo> BackgroundJobs { get; set; } = default!;

        // Notifications
        public DbSet<Notification> Notifications { get; set; } = default!;
        public DbSet<NotificationSubscription> NotificationSubscriptions { get; set; } = default!;

        // Languages
        public DbSet<ApplicationLanguage> Languages { get; set; } = default!;
        public DbSet<ApplicationLanguageText> LanguageTexts { get; set; } = default!;

        // Organization Units
        public DbSet<OrganizationUnit> OrganizationUnits { get; set; } = default!;
        public DbSet<UserOrganizationUnit> UserOrganizationUnits { get; set; } = default!;
        public DbSet<OrganizationUnitRole> OrganizationUnitRoles { get; set; } = default!;


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            //// Sys kernel mappings
            //builder.Entity<Tenant>().ToTable("SysTenants");
            //builder.Entity<Edition>().ToTable("SysEditions");
            //builder.Entity<Feature>().ToTable("SysFeatures");
            //builder.Entity<FeatureValue>().ToTable("SysFeatureValues");
            //builder.Entity<AuditLog>().ToTable("SysAuditLogs");
            //builder.Entity<Setting>().ToTable("SysSettings");
            //builder.Entity<BackgroundJobInfo>().ToTable("SysBackgroundJobs");

            //// Notifications
            //builder.Entity<Notification>().ToTable("SysNotifications");
            //builder.Entity<NotificationSubscription>().ToTable("SysNotificationSubscriptions");

            //// Languages
            //builder.Entity<ApplicationLanguage>().ToTable("SysLanguages");
            //builder.Entity<ApplicationLanguageText>().ToTable("SysLanguageTexts");

            //// Organization Units
            //builder.Entity<OrganizationUnit>().ToTable("SysOrganizationUnits");
            //builder.Entity<UserOrganizationUnit>().ToTable("SysUserOrganizationUnits");
            //builder.Entity<OrganizationUnitRole>().ToTable("SysOrganizationUnitRoles");

            // Relationships
            builder.Entity<Feature>()
                .HasOne(f => f.Edition)
                .WithMany(e => e.Features)
                .HasForeignKey(f => f.EditionId);

            builder.Entity<OrganizationUnit>()
                .HasMany(o => o.UserLinks)
                .WithOne()
                .HasForeignKey(u => u.OrganizationUnitId);

            builder.Entity<OrganizationUnit>()
                .HasMany(o => o.RoleLinks)
                .WithOne()
                .HasForeignKey(r => r.OrganizationUnitId);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<IFullAudited>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreationTime = DateTime.UtcNow;
                }
                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.LastModificationTime = DateTime.UtcNow;
                }
            }

            foreach (var entry in ChangeTracker.Entries<IDeletionAudited>())
            {
                if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    entry.Entity.IsDeleted = true;
                    entry.Entity.DeletionTime = DateTime.UtcNow;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
