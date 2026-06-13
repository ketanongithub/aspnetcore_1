using ENyayPath.PICS.Core.Dependency;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace ENyayPath.PICS.Core.Authorization.Permissions
{
    /// <summary>
    /// Represents a requirement for a specific permission.
    /// Used in policies to enforce RBAC.
    /// </summary>
    public class PermissionRequirement : IAuthorizationRequirement //, ITransientDependency
    {
        public string PermissionName { get; }

        public PermissionRequirement(string permissionName)
        {
            PermissionName = permissionName;
        }

        //public PermissionRequirement() { }
    }
}
