
✅ Implementation Plan (High‑Level)
Foundation → Solution structure + Domain + Application + EF Core project.

Identity & Security → Users, Roles, Permissions, JWT.

Multi‑Tenancy → Tenant entity + filters + tenant resolver middleware.

Audit & Logging → AuditLog entity + middleware + Serilog integration.

Config/Settings → Setting entity + service + tenant‑aware config.

Web/API → Controllers + Swagger + versioning.

Cross‑Cutting → Localization, caching, background jobs, notifications.

CI/CD → Migration pipeline + environment separation.


dotnet new sln -n ENyayPath.PICS

Step 1: Create the src Folder
mkdir src


Step 2: Add New Projects Inside src
cd src
# Core project (shared contracts, abstractions)
dotnet new classlib -n ENyayPath.PICS.Core

# Domain project (entities, domain services)
dotnet new classlib -n ENyayPath.PICS.Domain

# Application project (DTOs, services, mappings)
dotnet new classlib -n ENyayPath.PICS.Application

# Infrastructure project (EF Core DbContext, repositories, migrations)
dotnet new classlib -n ENyayPath.PICS.EntityFrameworkCore

# Web API project (controllers, middleware, startup)
dotnet new webapi -n ENyayPath.PICS.Web

dotnet new webapi -n ENyayPath.PICS.Web

Step 3: Add Projects to Solution
cd ..
dotnet sln add src/ENyayPath.PICS.Core/ENyayPath.PICS.Core.csproj
dotnet sln add src/ENyayPath.PICS.Application/ENyayPath.PICS.Application.csproj
dotnet sln add src/ENyayPath.PICS.Domain/ENyayPath.PICS.Domain.csproj
dotnet sln add src/ENyayPath.PICS.Infrastructure/ENyayPath.PICS.Infrastructure.csproj
dotnet sln add src/ENyayPath.PICS.EntityFrameworkCore/ENyayPath.PICS.EntityFrameworkCore.csproj
dotnet sln add src/ENyayPath.PICS.Web/ENyayPath.PICS.Web.csproj

Step 4: Wire Up References

# Domain depends on Core
dotnet add src/ENyayPath.PICS.Domain/ENyayPath.PICS.Domain.csproj reference src/ENyayPath.PICS.Core/ENyayPath.PICS.Core.csproj

# Application depends on Domain + Core
dotnet add src/ENyayPath.PICS.Application/ENyayPath.PICS.Application.csproj reference src/ENyayPath.PICS.Domain/ENyayPath.PICS.Domain.csproj
dotnet add src/ENyayPath.PICS.Application/ENyayPath.PICS.Application.csproj reference src/ENyayPath.PICS.Core/ENyayPath.PICS.Core.csproj

# EntityFrameworkCore depends on Domain + Core
dotnet add src/ENyayPath.PICS.EntityFrameworkCore/ENyayPath.PICS.EntityFrameworkCore.csproj reference src/ENyayPath.PICS.Domain/ENyayPath.PICS.Domain.csproj
dotnet add src/ENyayPath.PICS.EntityFrameworkCore/ENyayPath.PICS.EntityFrameworkCore.csproj reference src/ENyayPath.PICS.Core/ENyayPath.PICS.Core.csproj

# Web depends on Application + EntityFrameworkCore
dotnet add src/ENyayPath.PICS.Web/ENyayPath.PICS.Web.csproj reference src/ENyayPath.PICS.Application/ENyayPath.PICS.Application.csproj
dotnet add src/ENyayPath.PICS.Web/ENyayPath.PICS.Web.csproj reference src/ENyayPath.PICS.EntityFrameworkCore/ENyayPath.PICS.EntityFrameworkCore.csproj


Step 5: Add EF Core NuGet Packages
dotnet add src/ENyayPath.PICS.EntityFrameworkCore package Microsoft.EntityFrameworkCore
dotnet add src/ENyayPath.PICS.EntityFrameworkCore package Microsoft.EntityFrameworkCore.SqlServer
dotnet add src/ENyayPath.PICS.EntityFrameworkCore package Microsoft.EntityFrameworkCore.Design

Optional (for migrations tooling):
dotnet add src/ENyayPath.PICS.EntityFrameworkCore package Microsoft.EntityFrameworkCore.Tools


------------------------------------------
Script-Migration -Context PICSDbContext -From 20260608063009_PICS_InitialSysIdentity -To 20260608063009_PICS_InitialSysIdentity -Output Migration_20260608063009_PICS_InitialSysIdentity_Down.sql

Script-Migration -Context PICSDbContext -From 20260608063009_PICS_InitialSysIdentity -To 0 -Output Migration_InitialSysIdentity_Down.sql
------------------------------------------