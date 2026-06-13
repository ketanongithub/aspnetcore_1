using ENyayPath.PICS.Core.Authorization.Permissions;
using ENyayPath.PICS.Core.Features;
using ENyayPath.PICS.Core.Services;
using ENyayPath.PICS.Core.Sessions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ENyayPath.PICS.Application.Services
{
    /// <summary>
    /// Base class for application services.
    /// Provides session, permission, and feature management.
    /// </summary>
    public abstract class ApplicationService : AppServiceBase, IApplicationService, IAvoidDuplicateCrossCuttingConcerns
    {
        public static string[] CommonPostfixes = { "AppService", "ApplicationService" };

        /// <summary>
        /// Current session information.
        /// </summary>
        public IAppSession AppSession { get; }

        /// <summary>
        /// Permission manager for CRUD operations.
        /// </summary>
        protected IPermissionManager PermissionManager { get; }

        /// <summary>
        /// Permission checker for runtime enforcement.
        /// </summary>
        protected IPermissionChecker PermissionChecker { get; }

        /// <summary>
        /// Feature manager for tenant features.
        /// </summary>
        protected IFeatureManager FeatureManager { get; }

        /// <summary>
        /// Feature checker for runtime enforcement.
        /// </summary>
        protected IFeatureChecker FeatureChecker { get; }

        /// <summary>
        /// Applied cross cutting concerns.
        /// </summary>
        public List<string> AppliedCrossCuttingConcerns { get; } = new List<string>();

        protected ApplicationService(
            IAppSession appSession,
            IPermissionManager? permissionManager = null,
            IPermissionChecker? permissionChecker = null,
            IFeatureManager? featureManager = null,
            IFeatureChecker? featureChecker = null)
        {
            AppSession = appSession ?? NullAppSession.Instance;
            PermissionManager = permissionManager ?? NullPermissionManager.Instance;
            PermissionChecker = permissionChecker ?? NullPermissionChecker.Instance;
            FeatureManager = featureManager ?? NullFeatureManager.Instance;
            FeatureChecker = featureChecker ?? NullFeatureChecker.Instance;
        }

        /// <summary>
        /// Checks if current user is granted for a permission.
        /// </summary>
        protected virtual Task<bool> IsGrantedAsync(string permissionName) => PermissionChecker.IsGrantedAsync(permissionName);

        protected virtual bool IsGranted(string permissionName) => PermissionChecker.IsGranted(permissionName);

        /// <summary>
        /// Checks if given feature is enabled for current tenant.
        /// </summary>
        protected virtual Task<bool> IsEnabledAsync(string featureName) => FeatureChecker.IsEnabledAsync(featureName);

        protected virtual bool IsEnabled(string featureName) => FeatureChecker.IsEnabled(featureName);
    }
}
