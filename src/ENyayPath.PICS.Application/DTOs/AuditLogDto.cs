using ENyayPath.PICS.Core.Entities;
using ENyayPath.PICS.Core.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace ENyayPath.PICS.Application.DTOs
{
    public class AuditLogDto : Entity<long>, IMayHaveTenant
    {
        public long Id { get; set; }

        // Tenant context
        public int? TenantId { get; set; }

        // User context
        public int? UserId { get; set; }
        public string? UserName { get; set; }

        // Request context
        public DateTime ExecutionTime { get; set; } = DateTime.UtcNow;
        public int ExecutionDuration { get; set; } // in ms
        public string? ClientIpAddress { get; set; }
        public string? ClientName { get; set; } // e.g., machine name
        public string? BrowserInfo { get; set; }
        public string? Url { get; set; }
        public string? HttpMethod { get; set; }

        // Action context
        public string? ServiceName { get; set; } // e.g., ApplicationService
        public string? MethodName { get; set; }
        public string? Parameters { get; set; } // serialized JSON
        public string? ReturnValue { get; set; }

        // Exception context
        public string? Exception { get; set; }

        // Custom correlation
        public string? CorrelationId { get; set; }
        public string? Comments { get; set; }
    }
}
