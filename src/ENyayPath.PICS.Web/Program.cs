using ENyayPath.PICS.Application.Mappings;
using ENyayPath.PICS.Application.MultiTenancy;
using ENyayPath.PICS.Application.Settings;
using ENyayPath.PICS.Core.Authorization.Permissions;
using ENyayPath.PICS.Core.Authorization.Roles;
using ENyayPath.PICS.Core.Authorization.Users;
using ENyayPath.PICS.Core.Dependency;
using ENyayPath.PICS.Core.Localization;
using ENyayPath.PICS.Core.MultiTenancy;
using ENyayPath.PICS.Core.Repositories;
using ENyayPath.PICS.Core.Services;
using ENyayPath.PICS.Core.Sessions;
using ENyayPath.PICS.Core.Settings;
using ENyayPath.PICS.EntityFrameworkCore.DbContexts;
using ENyayPath.PICS.EntityFrameworkCore.Repositories;
using ENyayPath.PICS.EntityFrameworkCore.Seeds;
using ENyayPath.PICS.Web.Helpers;
using ENyayPath.PICS.Web.Sessions;
using ENyayPath.PICS.Web.Startup;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

var builder = WebApplication.CreateBuilder(args);

// Load appsettings.json first, then read the connection string
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
var connectionString = builder.Configuration.GetConnectionString("Default");

// Register DbContext with connection string
builder.Services.AddDbContext<PICSDbContext>(options =>
    options.UseSqlServer(connectionString));

// Identity
builder.Services.AddIdentity<User, Role>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireUppercase = true;
    options.User.RequireUniqueEmail = true;
    options.Password.RequireNonAlphanumeric = false;
})
.AddEntityFrameworkStores<PICSDbContext>()
.AddDefaultTokenProviders();

// Register repositories and unit of work
builder.Services.AddScoped(typeof(IRepository<,>), typeof(EfRepository<,>));

//builder.Services.AddScoped<IUnitOfWork, EfUnitOfWork>();
builder.Services.AddScoped<IAppSession, AppSession>();


//builder.Services.AddTransient<IAuthorizationRequirement, PermissionRequirement>();
//builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
//builder.Services.AddSingleton<IAuthorizationHandler, PermissionHandler>();

builder.Services.AddAuthorization(options =>
{
    foreach (var permission in PermissionNames.GetAll())
    {
        options.AddPolicy(permission, policy =>
            policy.Requirements.Add(new PermissionRequirement(permission)));
    }
});

builder.Services.AddSingleton<ILocalizationSource>(NullLocalizationSource.Instance);
builder.Services.AddSingleton<ILocalizationManager>(NullLocalizationManager.Instance);
builder.Services.AddScoped<IUnitOfWorkManager, EfCoreUnitOfWorkManager>();

// AutoMapper
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>();
});

// Auto-register application services by scanning project assemblies
UnifiedDependencyRegistrar.Register(builder.Services);

builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultCorsPolicy", policy =>
    {
        policy.WithOrigins(
                builder.Configuration.GetSection("App:CorsOrigins").Get<string[]>()
                    ?? new[] { "http://localhost:4200" })
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

AuthConfigurer.Configure(builder.Services, builder.Configuration);

var app = builder.Build();

app.UseRouting(); // If you are using routing middleware explicitly
app.UseCors("DefaultCorsPolicy");
// 1. AUTHENTICATION FIRST (Who are you?)
app.UseAuthentication();

// 2. AUTHORIZATION SECOND (Are you allowed here?)
app.UseAuthorization();

// 3. ENDPOINTS LAST (Execute the API)
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<PICSDbContext>();
    await SysDataSeeder.SeedAsync(dbContext);               // Editions, Tenants, Features
    await SysDataSeeder.SeedLanguagesAsync(dbContext);      // Languages + translations
    await SysDataSeeder.SeedSettingsAsync(dbContext);       // System settings
    await SysDataSeeder.DocumentAsync(dbContext);          // DocumentMaster
    await SysDataSeeder.SeedCountryAsync(dbContext);        // Countries    
    await SysDataSeeder.SeedStateAsync(dbContext);          //states
    await SysDataSeeder.SeedPrisonAsync(dbContext);      // Prisoners

    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
    await SysDataSeeder.SeedIdentityAsync(userManager, roleManager);    // Roles + Admin user
}

// Map AppService endpoints dynamically (auto-discover assemblies)
UnifiedDependencyRegistrar.MapAppServices(app);

//app.UseMiddleware<AuditLoggingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ENyayPath PICS API v1");
        c.RoutePrefix = string.Empty; // Swagger UI at root "/"
    });
}

app.Run();
