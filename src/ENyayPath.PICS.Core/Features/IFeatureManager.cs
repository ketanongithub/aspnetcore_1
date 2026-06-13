using ENyayPath.PICS.Core.Dependency;
using System;
using System.Collections.Generic;
using System.Text;

namespace ENyayPath.PICS.Core.Features
{
    /// <summary>
    /// Manages feature definitions and availability per tenant.
    /// </summary>
    public interface IFeatureManager : ITransientDependency
    {
        Task<bool> IsEnabledAsync(string featureName, int? tenantId = null);
        bool IsEnabled(string featureName, int? tenantId = null);
    }
}
