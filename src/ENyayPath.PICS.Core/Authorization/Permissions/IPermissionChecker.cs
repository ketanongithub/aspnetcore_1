using ENyayPath.PICS.Core.Dependency;
using System;
using System.Collections.Generic;
using System.Text;

namespace ENyayPath.PICS.Core.Authorization.Permissions
{
    /// <summary>
    /// Checks if current user is granted a permission.
    /// Bridges to Identity claims at runtime.
    /// </summary>
    public interface IPermissionChecker : ITransientDependency
    {
        Task<bool> IsGrantedAsync(string permissionName);
        bool IsGranted(string permissionName);
    }

    /// <summary>
    /// Null implementation for safe defaults.
    /// </summary>
    public class NullPermissionChecker : IPermissionChecker
    {
        public static NullPermissionChecker Instance { get; } = new NullPermissionChecker();

        public Task<bool> IsGrantedAsync(string permissionName) => Task.FromResult(false);
        public bool IsGranted(string permissionName) => false;
    }
}
