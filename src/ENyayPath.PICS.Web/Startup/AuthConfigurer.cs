using Azure;
using ENyayPath.PICS.Core.Authorization.Permissions;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENyayPath.PICS.Web.Startup
{
    public static class AuthConfigurer
    {
        public static void Configure(IServiceCollection services, IConfiguration configuration)
        {
            ConfigureAuth(services, configuration);
            ConfigureSwagger(services);
        }

        private static void ConfigureAuth(IServiceCollection services, IConfiguration configuration)
        {
            // Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true; // true in production with HTTPS
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Authentication:JwtBearer:Issuer"],
                    ValidAudience = configuration["Authentication:JwtBearer:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Authentication:JwtBearer:SecurityKey"]))
                };
            });

            // Authorization policies (example: dynamic permissions)
            services.AddAuthorization(options =>
            {
                foreach (var permission in PermissionNames.GetAll())
                {
                    //options.AddPolicy(permission, policy =>
                    //    policy.Requirements.Add(new PermissionRequirement(permission)));

                    options.AddPolicy(permission, policy =>
                    {
                        // ⚠️ CRITICAL: Ensure the policy requires an authenticated token first
                        policy.RequireAuthenticatedUser();

                        // Your existing permission/claim check logic goes here
                        //policy.RequireClaim("permission", permission);
                        policy.Requirements.Add(new PermissionRequirement(permission));
                    });
                }
            });

            // Register handlers
            services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
        }

        private static void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "ENyayPath PICS API",
                    Version = "v1",
                    Description = "🔐 Authentication Required for most endpoints.\n\n" +
                                  "**Quick Start:**\n" +
                                  "1. Go to the **Account** section\n" +
                                  "2. Call **POST /api/account/login** with username and password\n" +
                                  "3. Copy the returned token\n" +
                                  "4. Click the **Authorize** button (top right) and paste the token\n" +
                                  "5. All subsequent requests will include your token automatically",
                    Contact = new OpenApiContact
                    {
                        Name = "ENyayPath Team",
                        Email = "support@enyaypath.com"
                    }
                });

                // OAuth2 Password Grant Flow - Username/Password login
                // Note: This requires calling the Login endpoint first to get a token
                c.AddSecurityDefinition("OAuth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Password = new OpenApiOAuthFlow
                        {
                            // Swagger will show username/password fields
                            TokenUrl = new Uri("/api/account/login", UriKind.Relative),
                            Scopes = new Dictionary<string, string>
                            {
                                { "read", "Read access" },
                                { "write", "Write access" }
                            }
                        }
                    },
                    Description = "✅ INSTRUCTIONS:\n1. Click 'Authorize' button\n2. Enter username and password\n3. Token will be used automatically for protected endpoints\n\nAlternatively, call the Account/Login endpoint first to get a token, then paste it in the Bearer field below."
                });

                // JWT Bearer definition - for direct token input
                //c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                //{
                //    Description = "🔐 JWT Bearer token. Paste your token directly here (without 'Bearer' prefix). Get a token from the Account/Login endpoint.",
                //    Name = "Authorization",
                //    In = ParameterLocation.Header,
                //    Type = SecuritySchemeType.ApiKey,
                //    Scheme = "Bearer"
                //});
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,   // <-- Http, not ApiKey
                    Scheme = "bearer",                // <-- lowercase "bearer"
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Paste your JWT here. Example: 'Bearer {token}'"
                });

                // Global requirement - support both methods
                c.AddSecurityRequirement(document => new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("Bearer", document)] = new List<string>(),
                    [new OpenApiSecuritySchemeReference("OAuth2", document)] = new List<string> { "read", "write" }
                });

                //c.AddSecurityRequirement(new OpenApiSecurityRequirement
                //{
                //    {
                //        new OpenApiSecurityScheme
                //        {
                //            Reference = new OpenApiReference
                //            {
                //                Type = ReferenceType.SecurityScheme,
                //                Id = "Bearer"
                //            }
                //        },
                //        Array.Empty<string>()
                //    }
                //});

                //c.OperationFilter.Security.Add(new OpenApiSecurityRequirement
                //{
                //    [new OpenApiSecuritySchemeReference("Bearer", context.SchemaRepository.CurrentDocument)] = []
                //});
            });
        }
    }
}
