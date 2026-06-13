using ENyayPath.PICS.Core.Dependency;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace ENyayPath.PICS.Core.Authorization.Permissions
{
    /// <summary>
    /// Dynamic policy provider that creates policies for permissions on demand.
    /// This avoids hardcoding every permission in Startup.
    /// </summary>
    //public class PermissionPolicyProvider : IAuthorizationPolicyProvider, ITransientDependency
    //{
    //    private readonly DefaultAuthorizationPolicyProvider _fallbackPolicyProvider;

    //    public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
    //    {
    //        _fallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
    //    }

    //    public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
    //        => _fallbackPolicyProvider.GetDefaultPolicyAsync();

    //    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
    //        => _fallbackPolicyProvider.GetFallbackPolicyAsync();

    //    public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    //    {
    //        // Dynamically create a policy for any permission name
    //        var policy = new AuthorizationPolicyBuilder()
    //            .AddRequirements(new PermissionRequirement(policyName))
    //            .Build();

    //        return Task.FromResult<AuthorizationPolicy?>(policy);
    //    }
    //}

    public class PermissionPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        public PermissionPolicyProvider(IOptions<AuthorizationOptions> options) : base(options) { }

        public override Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            // If the policyName matches a permission, build a policy dynamically
            if (PermissionNames.GetAll().Contains(policyName))
            {
                var policy = new AuthorizationPolicyBuilder()
                    .AddRequirements(new PermissionRequirement(policyName))
                    .Build();

                return Task.FromResult(policy);
            }

            return base.GetPolicyAsync(policyName);
        }
    }
}
