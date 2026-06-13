using ENyayPath.PICS.Core.Dependency;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace ENyayPath.PICS.Core.Authorization.Permissions
{
    /// <summary>
    /// Custom authorization handler that enforces permissions.
    /// Supports role claims, user claims, and user-level deny overrides.
    /// </summary>
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            if (context.User.IsInRole("SuperAdmin"))
            {
                context.Succeed(requirement); // bypass claims
                return Task.CompletedTask;
            }

            // 1. Deny overrides: if user has a deny claim, block access
            if (context.User.HasClaim("Permission.Deny", requirement.PermissionName))
            {
                // Do not call context.Succeed → requirement fails
                return Task.CompletedTask;
            }

            // 2. Grant if user has permission claim (direct or via role)
            if (context.User.HasClaim("Permission", requirement.PermissionName))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
