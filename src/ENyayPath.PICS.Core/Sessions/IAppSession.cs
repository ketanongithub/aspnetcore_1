using ENyayPath.PICS.Core.Dependency;
using ENyayPath.PICS.Core.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace ENyayPath.PICS.Core.Sessions
{
    public interface IAppSession : ITransientDependency
    {
        //long? UserId { get; }
        //string? UserName { get; }
        //int? TenantId { get; }
        //string? TenantName { get; }

        //
        // Summary:
        //     Gets current UserId or null. It can be null if no user logged in.
        long? UserId { get; }

        //
        // Summary:
        //     Gets current TenantId or null. This TenantId should be the TenantId of the App.Runtime.Session.IAppSession.UserId.
        //     It can be null if given App.Runtime.Session.IAppSession.UserId is a host user
        //     or no user logged in.
        int? TenantId { get; }

        //
        // Summary:
        //     Gets current multi-tenancy side.
        MultiTenancySides MultiTenancySide { get; }

        //
        // Summary:
        //     UserId of the impersonator. This is filled if a user is performing actions behalf
        //     of the App.Runtime.Session.IAppSession.UserId.
        long? ImpersonatorUserId { get; }

        //
        // Summary:
        //     TenantId of the impersonator. This is filled if a user with App.Runtime.Session.IAppSession.ImpersonatorUserId
        //     performing actions behalf of the App.Runtime.Session.IAppSession.UserId.
        int? ImpersonatorTenantId { get; }

        //
        // Summary:
        //     Used to change App.Runtime.Session.IAppSession.TenantId and App.Runtime.Session.IAppSession.UserId
        //     for a limited scope.
        //
        // Parameters:
        //   tenantId:
        //
        //   userId:
        //IDisposable Use(int? tenantId, long? userId);
    }
}
