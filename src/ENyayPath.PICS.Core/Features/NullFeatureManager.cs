using ENyayPath.PICS.Core.Dependency;
using System;
using System.Collections.Generic;
using System.Text;

namespace ENyayPath.PICS.Core.Features
{
    /// <summary>
    /// Null implementation of IFeatureManager.
    /// Always returns false, meaning no features are enabled.
    /// </summary>
    public class NullFeatureManager : IFeatureManager, ITransientDependency
    {
        public static NullFeatureManager Instance { get; } = new NullFeatureManager();

        public NullFeatureManager() { }

        public Task<bool> IsEnabledAsync(string featureName, int? tenantId = null)
            => Task.FromResult(false);

        public bool IsEnabled(string featureName, int? tenantId = null)
            => false;
    }
}
