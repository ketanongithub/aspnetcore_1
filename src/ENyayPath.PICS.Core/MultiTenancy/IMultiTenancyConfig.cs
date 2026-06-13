using System;
using System.Collections.Generic;
using System.Text;

namespace ENyayPath.PICS.Core.MultiTenancy
{
    public interface IMultiTenancyConfig
    {
        //
        // Summary:
        //     Is multi-tenancy enabled? Default value: false.
        bool IsEnabled { get; set; }

        //
        // Summary:
        //     Ignore feature check for host users Default value: false.
        bool IgnoreFeatureCheckForHostUsers { get; set; }

        //
        // Summary:
        //     A list of contributors for tenant resolve process.
        //ITypeList<ITenantResolveContributor> Resolvers { get; }

        //
        // Summary:
        //     TenantId resolve key Default value: "TenantId"
        string TenantIdResolveKey { get; set; }
    }
}
