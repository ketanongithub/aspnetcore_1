using ENyayPath.PICS.Core.Auditing;
using ENyayPath.PICS.Core.Authorization.Roles;
using ENyayPath.PICS.Core.Authorization.Users;
using ENyayPath.PICS.Core.BackgroundJob;
using ENyayPath.PICS.Core.Editions;
using ENyayPath.PICS.Core.Eny.Common;
using ENyayPath.PICS.Core.Eny.Prisoner;
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

        // Prisoner
        public DbSet<Prisoner> Prisoners { get; set; } = default!;

        // Country
        public DbSet<CountryMaster> Countries { get; set; } = default!;

        // State
        public DbSet<StateMaster> States { get; set; } = default!;

        // City
        public DbSet<CityMaster> Cities { get; set; } = default!;

        // Lookup
        public DbSet<LookupMaster> Lookups { get; set; } = default!;

        // Document
        public DbSet<DocumentMaster> Documents { get; set; } = default!;

        // Prisoner related
        public DbSet<PrisonerBiometricData> PrisonerBiometricData { get; set; } = default!;
        public DbSet<PrisonerContactPerson> PrisonerContactPersons { get; set; } = default!;
        public DbSet<PrisonerContactDetail> PrisonerContactDetails { get; set; } = default!;
        public DbSet<PrisonerContactPersonDocument> PrisonerContactPersonDocuments { get; set; } = default!;
        public DbSet<PrisonalContactApprovalProcess> ContactApprovalProcesses { get; set; } = default!;
        public DbSet<PrisonerCallRecord> PrisonerCallRecords { get; set; } = default!;
        public DbSet<Recharge> Recharges { get; set; } = default!;
        public DbSet<Wallet> Wallets { get; set; } = default!;
        public DbSet<PrisonerDocument> PrisonerDocuments { get; set; } = default!;

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

            // Country unique constraint
            builder.Entity<CountryMaster>()
                .HasIndex(c => c.CountryCode)
                .IsUnique();

            // State relationships and constraints
            builder.Entity<StateMaster>()
                .HasOne(s => s.Country)
                .WithMany()
                .HasForeignKey(s => s.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<StateMaster>()
                .HasIndex(s => s.StateCode)
                .IsUnique();

            // City relationships
            builder.Entity<CityMaster>()
                .HasOne(c => c.Country)
                .WithMany()
                .HasForeignKey(c => c.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<CityMaster>()
                .HasOne(c => c.State)
                .WithMany()
                .HasForeignKey(c => c.StateId)
                .OnDelete(DeleteBehavior.Restrict);

            // Prisoner unique index on PrisonerId
            builder.Entity<Prisoner>()
                .HasIndex(p => p.PrisonerId)
                .IsUnique();

            // PrisonerBiometricData FK relationships
            builder.Entity<PrisonerBiometricData>()
                .HasOne<Prisoner>()
                .WithMany()
                .HasForeignKey(b => b.PrisonerId)
                .HasPrincipalKey(p => p.PrisonerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<PrisonerBiometricData>()
                .HasOne<LookupMaster>()
                .WithMany()
                .HasForeignKey(b => b.AuthenticationType)
                .OnDelete(DeleteBehavior.Restrict);

            // PrisonerContactPerson FK
            builder.Entity<PrisonerContactPerson>()
                .HasOne<Prisoner>()
                .WithMany()
                .HasForeignKey(c => c.PrisonerId)
                .HasPrincipalKey(p => p.PrisonerId)
                .OnDelete(DeleteBehavior.Restrict);

            // PrisonerContactDetail FK
            builder.Entity<PrisonerContactDetail>()
                .HasOne<PrisonerContactPerson>()
                .WithMany()
                .HasForeignKey(d => d.PrisonerContactPersonId)
                .OnDelete(DeleteBehavior.Restrict);

            // PrisonerContactPersonDocument FKs
            builder.Entity<PrisonerContactPersonDocument>()
                .HasOne<PrisonerContactPerson>()
                .WithMany()
                .HasForeignKey(d => d.PrisonerContactPersonId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<PrisonerContactPersonDocument>()
                .HasOne<DocumentMaster>()
                .WithMany()
                .HasForeignKey(d => d.DocumentId)
                .OnDelete(DeleteBehavior.Restrict);

            // PrisonalContactApprovalProcess FKs
            builder.Entity<PrisonalContactApprovalProcess>()
                .HasOne<PrisonerContactPersonDocument>()
                .WithMany()
                .HasForeignKey(a => a.PrisonerContactPersonDocumentId)
                .OnDelete(DeleteBehavior.Restrict);

            // PrisonerCallRecord FKs
            builder.Entity<PrisonerCallRecord>()
                .HasOne<PrisonerContactDetail>()
                .WithMany()
                .HasForeignKey(r => r.PrisonerContactDetailsId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<PrisonerCallRecord>()
                .HasOne<LookupMaster>()
                .WithMany()
                .HasForeignKey(r => r.TypeOfCall)
                .OnDelete(DeleteBehavior.Restrict);

            // Recharge FKs
            builder.Entity<Recharge>()
                .HasOne<Prisoner>()
                .WithMany()
                .HasForeignKey(r => r.PrisonerId)
                .HasPrincipalKey(p => p.PrisonerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Recharge>()
                .HasOne<LookupMaster>()
                .WithMany()
                .HasForeignKey(r => r.RechargeType)
                .OnDelete(DeleteBehavior.Restrict);

            // Wallet FK
            builder.Entity<Wallet>()
                .HasOne<Prisoner>()
                .WithMany()
                .HasForeignKey(w => w.PrisonerId)
                .HasPrincipalKey(p => p.PrisonerId)
                .OnDelete(DeleteBehavior.Restrict);
           
            // PrisonerDocument
            builder.Entity<PrisonerDocument>()
                .HasOne<Prisoner>()
                .WithMany()
                .HasForeignKey(d => d.PrisonerId)
                .HasPrincipalKey(p => p.PrisonerId)
                .OnDelete(DeleteBehavior.Restrict);
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
