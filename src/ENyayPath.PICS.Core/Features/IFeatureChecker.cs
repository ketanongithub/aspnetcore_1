using ENyayPath.PICS.Core.Dependency;
using System;
using System.Collections.Generic;
using System.Text;

namespace ENyayPath.PICS.Core.Features
{
    /// <summary>
    /// Checks if a feature is enabled for current tenant/session.
    /// </summary>
    public interface IFeatureChecker : ITransientDependency
    {
        Task<bool> IsEnabledAsync(string featureName);
        bool IsEnabled(string featureName);
    }

    /// <summary>
    /// Null implementation for safe defaults.
    /// </summary>
    public class NullFeatureChecker : IFeatureChecker
    {
        public static NullFeatureChecker Instance { get; } = new NullFeatureChecker();

        public Task<bool> IsEnabledAsync(string featureName) => Task.FromResult(false);
        public bool IsEnabled(string featureName) => false;
    }
}
