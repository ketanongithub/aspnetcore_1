using ENyayPath.PICS.Core.Authorization.Permissions;
using ENyayPath.PICS.Core.Authorization.Roles;
using ENyayPath.PICS.Core.Authorization.Users;
using ENyayPath.PICS.Core.Editions;
using ENyayPath.PICS.Core.Features;
using ENyayPath.PICS.Core.Localization;
using ENyayPath.PICS.Core.MultiTenancy;
using ENyayPath.PICS.Core.Settings;
using ENyayPath.PICS.EntityFrameworkCore.DbContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace ENyayPath.PICS.EntityFrameworkCore.Seeds
{
    public static class SysDataSeeder
    {

        public static async Task SeedAsync(PICSDbContext dbContext)
        {
            await dbContext.Database.MigrateAsync();

            // Editions
            if (!dbContext.Editions.Any())
            {
                var freeEdition = new Edition
                {
                    Name = "Free",
                    DisplayName = "Free Edition",
                    Description = "Basic free tier",
                    PricePerMonth = 0,
                    PricePerYear = 0,
                    IsActive = true
                };

                var standardEdition = new Edition
                {
                    Name = "Standard",
                    DisplayName = "Standard Edition",
                    Description = "Standard subscription tier",
                    PricePerMonth = 49,
                    PricePerYear = 499,
                    IsActive = true
                };

                var enterpriseEdition = new Edition
                {
                    Name = "Enterprise",
                    DisplayName = "Enterprise Edition",
                    Description = "Enterprise tier with advanced features",
                    PricePerMonth = 199,
                    PricePerYear = 1999,
                    IsActive = true
                };

                dbContext.Editions.AddRange(freeEdition, standardEdition, enterpriseEdition);
                await dbContext.SaveChangesAsync();

                // Features for Free Edition
                dbContext.Features.AddRange(
                    new Feature { Name = "MaxUsers", DisplayName = "Maximum Users", ValueType = "int", DefaultValue = "5", EditionId = freeEdition.Id },
                    new Feature { Name = "StorageLimit", DisplayName = "Storage Limit (MB)", ValueType = "int", DefaultValue = "500", EditionId = freeEdition.Id }
                );

                // Features for Standard Edition
                dbContext.Features.AddRange(
                    new Feature { Name = "MaxUsers", DisplayName = "Maximum Users", ValueType = "int", DefaultValue = "50", EditionId = standardEdition.Id },
                    new Feature { Name = "StorageLimit", DisplayName = "Storage Limit (MB)", ValueType = "int", DefaultValue = "5000", EditionId = standardEdition.Id },
                    new Feature { Name = "VideoConferencing", DisplayName = "Video Conferencing", ValueType = "bool", DefaultValue = "true", EditionId = standardEdition.Id }
                );

                // Features for Enterprise Edition
                dbContext.Features.AddRange(
                    new Feature { Name = "MaxUsers", DisplayName = "Maximum Users", ValueType = "int", DefaultValue = "500", EditionId = enterpriseEdition.Id },
                    new Feature { Name = "StorageLimit", DisplayName = "Storage Limit (MB)", ValueType = "int", DefaultValue = "50000", EditionId = enterpriseEdition.Id },
                    new Feature { Name = "VideoConferencing", DisplayName = "Video Conferencing", ValueType = "bool", DefaultValue = "true", EditionId = enterpriseEdition.Id },
                    new Feature { Name = "AdvancedAnalytics", DisplayName = "Advanced Analytics", ValueType = "bool", DefaultValue = "true", EditionId = enterpriseEdition.Id }
                );

                await dbContext.SaveChangesAsync();

                // Default Tenant linked to Standard Edition
                if (!dbContext.Tenants.Any())
                {
                    var defaultTenant = new Tenant
                    {
                        TenancyName = "Default Tenant",
                        Code = "DEFAULT",
                        Description = "System default tenant",
                        IsActive = true,
                        ActivationDate = DateTime.UtcNow,
                        SubscriptionPlan = "Standard",
                        SubscriptionExpiry = DateTime.UtcNow.AddYears(1),
                        Theme = "Default",
                        LogoUrl = null,
                        Edition = freeEdition
                    };

                    dbContext.Tenants.Add(defaultTenant);
                    await dbContext.SaveChangesAsync();
                }
            }
        }

        public static async Task SeedIdentityAsync(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            // --- Ensure SuperAdmin Role ---
            var superAdminRole = await roleManager.FindByNameAsync("SuperAdmin");
            if (superAdminRole == null)
            {
                await roleManager.CreateAsync(new Role { Name = "SuperAdmin", Description = "System SuperAdmin" });
            }

            // --- Ensure SuperAdmin User ---
            var superAdminUser = await userManager.FindByNameAsync("superadmin");
            if (superAdminUser == null)
            {
                superAdminUser = new User
                {
                    UserName = "superadmin",
                    Email = "superadmin@default.local",
                    TenantId = 1,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(superAdminUser, "SuperAdmin@123"); // strong default password
                await userManager.AddToRoleAsync(superAdminUser, "SuperAdmin");
            }

            // Create Admin role
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new Role { Name = "Admin", Description = "System Administrator" });
            }

            // Create Admin user
            var adminUser = await userManager.FindByEmailAsync("admin@default.local");
            if (adminUser == null)
            {
                adminUser = new User
                {
                    UserName = "admin",
                    Email = "admin@default.local",
                    TenantId = 1,
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(adminUser, "Admin@123"); // strong default password
                await userManager.AddToRoleAsync(adminUser, "Admin");

                // Ensure Admin role exists
                var adminRole = await roleManager.FindByNameAsync("Admin");
                if (adminRole == null)
                {
                    adminRole = new Role { Name = "Admin", NormalizedName = "ADMIN" };
                    await roleManager.CreateAsync(adminRole);
                }

                // --- Seed Permissions for Admin Role ---

                foreach (var permission in PermissionNames.GetAll())
                {
                    var claims = await roleManager.GetClaimsAsync(adminRole);
                    if (!claims.Any(c => c.Type == "Permission" && c.Value == permission))
                    {
                        await roleManager.AddClaimAsync(adminRole, new Claim("Permission", permission));
                    }
                }
            }
        }

        public static async Task SeedLanguagesAsync(PICSDbContext dbContext)
        {
            if (!dbContext.Languages.Any())
            {
                var english = new ApplicationLanguage
                {
                    Name = "en",
                    DisplayName = "English",
                    Icon = "flag-icon-us",
                    IsDisabled = false
                };

                var hindi = new ApplicationLanguage
                {
                    Name = "hi",
                    DisplayName = "Hindi",
                    Icon = "flag-icon-in",
                    IsDisabled = false
                };

                var french = new ApplicationLanguage
                {
                    Name = "fr",
                    DisplayName = "French",
                    Icon = "flag-icon-fr",
                    IsDisabled = false
                };

                dbContext.Languages.AddRange(english, hindi, french);
                await dbContext.SaveChangesAsync();

                // Seed sample translations
                dbContext.LanguageTexts.AddRange(
                    new ApplicationLanguageText { LanguageName = "en", Source = "UI", Key = "Hello", Value = "Hello" },
                    new ApplicationLanguageText { LanguageName = "hi", Source = "UI", Key = "Hello", Value = "नमस्ते" },
                    new ApplicationLanguageText { LanguageName = "fr", Source = "UI", Key = "Hello", Value = "Bonjour" }
                );

                await dbContext.SaveChangesAsync();
            }
        }

        public static async Task SeedSettingsAsync(PICSDbContext dbContext)
        {
            if (!dbContext.Settings.Any())
            {
                var systemSettings = new List<Setting>
                {
                    new Setting
                    {
                        Name = "Theme",
                        Value = "Default",
                        ValueType = "string",
                        IsSystemSetting = true,
                        LastModificationTime = DateTime.UtcNow
                    },
                    new Setting
                    {
                        Name = "TimeZone",
                        Value = "Asia/Kolkata",
                        ValueType = "string",
                        IsSystemSetting = true,
                        LastModificationTime = DateTime.UtcNow
                    },
                    new Setting
                    {
                        Name = "Language",
                        Value = "en",
                        ValueType = "string",
                        IsSystemSetting = true,
                        LastModificationTime = DateTime.UtcNow
                    }
                };

                dbContext.Settings.AddRange(systemSettings);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}