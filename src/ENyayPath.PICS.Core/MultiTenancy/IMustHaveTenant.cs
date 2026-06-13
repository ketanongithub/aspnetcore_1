using System;
using System.Collections.Generic;
using System.Text;

namespace ENyayPath.PICS.Core.MultiTenancy
{
    /// <summary>
    /// Mandatory tenant association.
    /// </summary>
    public interface IMustHaveTenant
    {
        int TenantId { get; set; }
    }
}
