using System;
using System.Collections.Generic;
using System.Text;

namespace ENyayPath.PICS.Core.MultiTenancy
{
    [Flags]
    public enum MultiTenancySides
    {
        //
        // Summary:
        //     Tenant side.
        Tenant = 1,
        //
        // Summary:
        //     Host (tenancy owner) side.
        Host = 2
    }
}
