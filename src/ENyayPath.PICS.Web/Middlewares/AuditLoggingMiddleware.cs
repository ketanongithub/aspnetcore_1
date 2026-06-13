//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Logging;
//using System.Diagnostics;
//using System.Text.Json;
//using ENyayPath.PICS.Core.Entities;
//using ENyayPath.PICS.EntityFrameworkCore.DbContexts;
//using ENyayPath.PICS.Core.Auditing;

//namespace ENyayPath.PICS.Web.Middlewares
//{
//    private readonly RequestDelegate _next;
//    private readonly ILogger<AuditLoggingMiddleware> _logger;

//    public AuditLoggingMiddleware(RequestDelegate next, ILogger<AuditLoggingMiddleware> logger)
//    {
//        _next = next;
//        _logger = logger;
//    }

//    public async Task InvokeAsync(HttpContext context, PICSDbContext dbContext)
//    {
//        var stopwatch = Stopwatch.StartNew();
//        var auditLog = new AuditLog
//        {
//            TenantId = context.User?.Claims.FirstOrDefault(c => c.Type == "TenantId")?.Value is string tid ? int.Parse(tid) : null,
//            UserId = context.User?.Identity?.IsAuthenticated == true
//                ? int.Parse(context.User.Claims.First(c => c.Type == "UserId").Value)
//                : null,
//            UserName = context.User?.Identity?.Name,
//            ExecutionTime = DateTime.UtcNow,
//            ClientIpAddress = context.Connection.RemoteIpAddress?.ToString(),
//            BrowserInfo = context.Request.Headers["User-Agent"].ToString(),
//            Url = context.Request.Path,
//            HttpMethod = context.Request.Method,
//            ServiceName = "HTTP Request",
//            MethodName = context.Request.Method,
//            Parameters = JsonSerializer.Serialize(new
//            {
//                Query = context.Request.Query.ToDictionary(q => q.Key, q => q.Value.ToString()),
//                Body = context.Request.ContentLength > 0 ? "[Captured separately]" : null
//            }),
//            CorrelationId = context.TraceIdentifier
//        };

//        try
//        {
//            await _next(context);
//            stopwatch.Stop();
//            auditLog.ExecutionDuration = (int)stopwatch.ElapsedMilliseconds;
//            auditLog.ReturnValue = $"StatusCode: {context.Response.StatusCode}";
//        }
//        catch (Exception ex)
//        {
//            stopwatch.Stop();
//            auditLog.ExecutionDuration = (int)stopwatch.ElapsedMilliseconds;
//            auditLog.Exception = ex.ToString();
//            throw; // rethrow after logging
//        }
//        finally
//        {
//            try
//            {
//                dbContext.AuditLogs.Add(auditLog);
//                await dbContext.SaveChangesAsync();
//            }
//            catch (Exception logEx)
//            {
//                _logger.LogError(logEx, "Failed to save audit log");
//            }
//        }
//    }
//}
