using ENyayPath.PICS.Core.Dependency;
using ENyayPath.PICS.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;

namespace ENyayPath.PICS.Web.Helpers
{
    /// <summary>
    /// Robust dependency registrar and dynamic AppService mapper.
    /// Scans assemblies (auto-loads project DLLs), registers implementations using marker interfaces
    /// or naming conventions, avoids duplicate registrations, and maps AppService endpoints.
    /// </summary>
    public static class UnifiedDependencyRegistrar
    {
        private static readonly string[] DefaultAssemblyPrefixes = new[] { "ENyayPath.PICS" };

        public static void Register(IServiceCollection services, params Assembly[] assemblies)
        {
            assemblies = EnsureAssemblies(assemblies);

            var markerInterfaces = new[] { typeof(ITransientDependency), typeof(IScopedDependency), typeof(ISingletonDependency) };

            var types = assemblies.SelectMany(a => SafeGetTypes(a)).Where(t => t.IsClass && !t.IsAbstract).ToArray();

            foreach (var impl in types)
            {
                if (impl.IsGenericType) continue;
                if (impl.IsNestedPrivate) continue;
                if (impl.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), false).Any()) continue;

                if (typeof(IAuthorizationRequirement).IsAssignableFrom(impl))
                {
                    continue; // don't register requirements
                }
                if (typeof(IActiveUnitOfWork).IsAssignableFrom(impl))
                {
                    continue; // skip, don't register
                }

                var implemented = impl.GetInterfaces().Where(i => !markerInterfaces.Contains(i) && i != typeof(IDisposable)).ToArray();

                var lifetime = ResolveLifetime(impl);

                if (implemented.Length > 0)
                {
                    foreach (var iface in implemented)
                    {
                        if (iface.Namespace != null && iface.Namespace.StartsWith("System")) continue;
                        if (services.Any(sd => sd.ServiceType == iface)) continue;
                        Add(services, iface, impl, lifetime);
                    }
                }
                else
                {
                    // Register concrete by convention
                    if (impl.Name.EndsWith("AppService") || impl.Name.EndsWith("Manager") || impl.Name.EndsWith("Repository") || impl.Name.EndsWith("Service"))
                    {
                        if (!services.Any(sd => sd.ServiceType == impl))
                        {
                            Add(services, impl, impl, lifetime);
                        }
                    }
                }
            }
        }

        public static void MapAppServices(WebApplication app, params Assembly[] assemblies)
        {
            assemblies = EnsureAssemblies(assemblies);

            var mapped = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var discoveredServices = new List<(string Service, string Interface)>();

            foreach (var asm in assemblies)
            {
                var appServices = SafeGetTypes(asm).Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("AppService"));

                foreach (var impl in appServices)
                {
                    // Look for interface implementing the app service
                    // Try multiple matching strategies:
                    // 1. Exact match: IServiceNameAppService
                    // 2. Interface contains the service name: ISettingsAppService for SettingAppService
                    var serviceName = impl.Name.Replace("AppService", "");
                    var iface = impl.GetInterfaces()
                        .FirstOrDefault(i => i.Name.StartsWith("I") && 
                                            i.Namespace != null &&
                                            !i.Namespace.StartsWith("System") &&
                                            (i.Name == $"I{impl.Name}" ||  // Exact match
                                             i.Name == $"I{serviceName}AppService" ||  // Base name match
                                             i.Name.Contains(serviceName)));  // Contains name
                    
                    if (iface == null)
                    {
                        discoveredServices.Add((impl.Name, "NOT FOUND"));
                        continue;
                    }

                    discoveredServices.Add((impl.Name, iface.Name));

                    var methods = iface.GetMethods();
                    foreach (var method in methods)
                    {
                        var route = $"/api/{impl.Name.Replace("AppService", "").ToLower()}/{method.Name.ToLower()}";
                        if (mapped.Contains(route)) continue;
                        mapped.Add(route);

                        var parameters = method.GetParameters();
                        bool hasFormFile = parameters.Any(p => typeof(IFormFile).IsAssignableFrom(p.ParameterType));
                        bool useBody = !hasFormFile && parameters.Length == 1 && !IsSimple(parameters[0].ParameterType);
                        var verb = GetHttpVerb(method, useBody);
                        
                        // Create a tag for Swagger grouping based on service name
                        var tag = impl.Name.Replace("AppService", "");
                        var operationId = $"{impl.Name.Replace("AppService", "")}_{method.Name}";
                        var returnType = GetUnwrappedReturnType(method.ReturnType);
                        
                        // Build parameter description
                        var paramDesc = string.Join(", ", parameters.Select(p => $"{p.ParameterType.Name} {p.Name}"));
                        var description = $"Method: {method.Name}\nParameters: {(string.IsNullOrEmpty(paramDesc) ? "None" : paramDesc)}";

                        // Extract authorization requirements
                        var classAuthAttr = impl.GetCustomAttribute<AuthorizeAttribute>();
                        var methodAuthAttr = method.GetCustomAttribute<AuthorizeAttribute>();
                        var authAttr = methodAuthAttr ?? classAuthAttr;  // Method-level overrides class-level

                        if (hasFormFile)
                        {
                            var mapPost = app.MapPost(route, GetFormDataHandler(method, parameters, iface, impl))
                            .WithName(operationId)
                            .WithTags(tag)
                            .WithDescription(description)
                            .WithSummary(method.Name)
                            .Accepts<IFormFile>("multipart/form-data")
                            .Produces(200, returnType)
                            .DisableAntiforgery()
                            .WithOpenApi();

                            if (authAttr != null)
                            {
                                if (!string.IsNullOrEmpty(authAttr.Policy))
                                {
                                    mapPost.RequireAuthorization(authAttr.Policy);
                                }
                                else
                                {
                                    mapPost.RequireAuthorization();
                                }
                            }
                        }
                        else if (verb == "GET")
                        {
                            var mapGet = app.MapGet(route, GetHandler(method, parameters, iface, impl))
                            .WithName(operationId)
                            .WithTags(tag)
                            .WithDescription(description)
                            .WithSummary(method.Name)
                            .Produces(200, returnType)
                            .WithOpenApi();

                            // Apply authorization if attribute is present
                            if (authAttr != null)
                            {
                                if (!string.IsNullOrEmpty(authAttr.Policy))
                                {
                                    mapGet.RequireAuthorization(authAttr.Policy);
                                }
                                else
                                {
                                    mapGet.RequireAuthorization();
                                }
                            }
                        }
                        else
                        {
                            // POST request - map with body parameter if needed
                            if (useBody && parameters.Length > 0)
                            {
                                var bodyType = parameters[0].ParameterType;
                                var postHandler = GetPostHandlerWithBody(method, parameters, iface, impl, bodyType);
                                var mapPost = app.MapPost(route, postHandler)
                                .WithName(operationId)
                                .WithTags(tag)
                                .WithDescription(description)
                                .WithSummary(method.Name)
                                .Accepts(bodyType, "application/json")
                                .Produces(200, returnType)
                                .WithOpenApi();

                                // Apply authorization if attribute is present
                                if (authAttr != null)
                                {
                                    if (!string.IsNullOrEmpty(authAttr.Policy))
                                    {
                                        mapPost.RequireAuthorization(authAttr.Policy);
                                    }
                                    else
                                    {
                                        mapPost.RequireAuthorization();
                                    }
                                }
                            }
                            else
                            {
                                // POST without body parameters
                                var mapPost = app.MapPost(route, GetHandler(method, parameters, iface, impl))
                                .WithName(operationId)
                                .WithTags(tag)
                                .WithDescription(description)
                                .WithSummary(method.Name)
                                .Produces(200, returnType)
                                .WithOpenApi();

                                // Apply authorization if attribute is present
                                if (authAttr != null)
                                {
                                    if (!string.IsNullOrEmpty(authAttr.Policy))
                                    {
                                        mapPost.RequireAuthorization(authAttr.Policy);
                                    }
                                    else
                                    {
                                        mapPost.RequireAuthorization();
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // Log discovered services to console
            if (discoveredServices.Any())
            {
                System.Console.WriteLine("=== AppServices Discovered ===");
                foreach (var (service, iface) in discoveredServices)
                {
                    System.Console.WriteLine($"  {service} -> {iface}");
                }
                System.Console.WriteLine($"Total routes mapped: {mapped.Count}");
            }
        }

        private static Delegate GetHandler(MethodInfo method, ParameterInfo[] parameters, Type iface, Type impl)
        {
            return new Func<HttpContext, IServiceProvider, Task<IResult>>(async (ctx, sp) =>
            {
                var service = sp.GetService(iface) ?? ActivatorUtilities.CreateInstance(sp, impl);
                var args = parameters.Select(p => ConvertQuery(ctx, p)).ToArray();
                var result = method.Invoke(service, args);
                return await ToResult(result);
            });
        }

        private static Delegate GetFormDataHandler(MethodInfo method, ParameterInfo[] parameters, Type iface, Type impl)
        {
            return new Func<HttpContext, IServiceProvider, Task<IResult>>(async (ctx, sp) =>
            {
                var service = sp.GetService(iface) ?? ActivatorUtilities.CreateInstance(sp, impl);
                var form = await ctx.Request.ReadFormAsync();
                var args = new object[parameters.Length];

                for (int i = 0; i < parameters.Length; i++)
                {
                    var p = parameters[i];
                    if (typeof(IFormFile).IsAssignableFrom(p.ParameterType))
                    {
                        args[i] = form.Files.GetFile(p.Name) ?? form.Files.FirstOrDefault();
                    }
                    else if (!IsSimple(p.ParameterType))
                    {
                        var dto = Activator.CreateInstance(p.ParameterType);
                        foreach (var prop in p.ParameterType.GetProperties())
                        {
                            var key = form.Keys.FirstOrDefault(k => k.Equals(prop.Name, StringComparison.OrdinalIgnoreCase));
                            if (key == null) continue;
                            var formValue = form[key].FirstOrDefault();
                            if (formValue == null) continue;

                            if (prop.PropertyType == typeof(Guid))
                                prop.SetValue(dto, Guid.Parse(formValue));
                            else if (prop.PropertyType == typeof(string))
                                prop.SetValue(dto, formValue);
                            else if (prop.PropertyType == typeof(int))
                                prop.SetValue(dto, int.Parse(formValue));
                            else if (prop.PropertyType == typeof(bool))
                                prop.SetValue(dto, bool.Parse(formValue));
                            else
                                prop.SetValue(dto, Convert.ChangeType(formValue, prop.PropertyType));
                        }
                        args[i] = dto!;
                    }
                    else
                    {
                        var key = form.Keys.FirstOrDefault(k => k.Equals(p.Name, StringComparison.OrdinalIgnoreCase));
                        var val = key != null ? form[key].FirstOrDefault() : null;
                        args[i] = val != null ? Convert.ChangeType(val, p.ParameterType)
                            : (p.ParameterType.IsValueType ? Activator.CreateInstance(p.ParameterType)! : null!);
                    }
                }

                var result = method.Invoke(service, args);
                return await ToResult(result);
            });
        }

        private static Delegate GetPostHandlerWithBody(MethodInfo method, ParameterInfo[] parameters, Type iface, Type impl, Type bodyType)
        {
            return new Func<HttpContext, IServiceProvider, Task<IResult>>(async (ctx, sp) =>
            {
                var service = sp.GetService(iface) ?? ActivatorUtilities.CreateInstance(sp, impl);
                object[] args;
                
                var dto = await ctx.Request.ReadFromJsonAsync(bodyType);
                args = new[] { dto! };
                
                var result = method.Invoke(service, args);
                return await ToResult(result);
            });
        }

        private static Assembly[] EnsureAssemblies(Assembly[] assemblies)
        {
            if (assemblies != null && assemblies.Length > 0) return assemblies.Distinct().ToArray();

            // Try to load known assemblies and DLLs from base directory
            try
            {
                var known = new[] { "ENyayPath.PICS.Core", "ENyayPath.PICS.Application", "ENyayPath.PICS.EntityFrameworkCore", "ENyayPath.PICS.Web" };
                foreach (var name in known)
                {
                    try { if (!AppDomain.CurrentDomain.GetAssemblies().Any(a => a.GetName().Name == name)) Assembly.Load(new AssemblyName(name)); }
                    catch { }
                }

                var baseDir = AppContext.BaseDirectory;
                var dlls = Directory.GetFiles(baseDir, "*.dll", SearchOption.TopDirectoryOnly)
                    .Where(f => DefaultAssemblyPrefixes.Any(p => Path.GetFileName(f).StartsWith(p, StringComparison.OrdinalIgnoreCase)));

                foreach (var dll in dlls)
                {
                    try
                    {
                        var asmName = AssemblyLoadContext.GetAssemblyName(dll);
                        if (!AppDomain.CurrentDomain.GetAssemblies().Any(a => a.GetName().Name == asmName.Name))
                            AssemblyLoadContext.Default.LoadFromAssemblyPath(dll);
                    }
                    catch { }
                }
            }
            catch { }

            return AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.IsDynamic && a.GetName().Name != null && DefaultAssemblyPrefixes.Any(p => a.GetName().Name.StartsWith(p)))
                .ToArray();
        }

        private static IEnumerable<Type> SafeGetTypes(Assembly asm)
        {
            try { return asm.GetTypes(); }
            catch { return Enumerable.Empty<Type>(); }
        }

        private static ServiceLifetime ResolveLifetime(Type impl)
        {
            if (typeof(ITransientDependency).IsAssignableFrom(impl)) return ServiceLifetime.Transient;
            if (typeof(IScopedDependency).IsAssignableFrom(impl)) return ServiceLifetime.Scoped;
            if (typeof(ISingletonDependency).IsAssignableFrom(impl)) return ServiceLifetime.Singleton;
            if (impl.Name.EndsWith("AppService") || impl.Name.EndsWith("Manager") || impl.Name.EndsWith("Repository") || impl.Name.EndsWith("Service")) return ServiceLifetime.Scoped;
            return ServiceLifetime.Transient;
        }

        private static void Add(IServiceCollection services, Type serviceType, Type implType, ServiceLifetime lifetime)
        {
            // Avoid duplicate registrations for same service type
            if (services.Any(sd => sd.ServiceType == serviceType)) return;
            var descriptor = new ServiceDescriptor(serviceType, implType, lifetime);
            services.Add(descriptor);
        }

        private static bool IsSimple(Type t) => t.IsPrimitive || t == typeof(string) || t == typeof(decimal) || t == typeof(DateTime);

        //private static string GetHttpVerb(string methodName, bool useBody)
        //{
        //    if (methodName.StartsWith("Get", StringComparison.OrdinalIgnoreCase)) return "GET";
        //    if (methodName.StartsWith("Delete", StringComparison.OrdinalIgnoreCase)) return "DELETE";
        //    if (useBody) return "POST";
        //    return "POST";
        //}
        private static string GetHttpVerb(MethodInfo method, bool useBody)
        {
            if (method.GetCustomAttribute<HttpPostAttribute>() != null) return "POST";
            if (method.GetCustomAttribute<HttpGetAttribute>() != null) return "GET";
            if (method.GetCustomAttribute<HttpDeleteAttribute>() != null) return "DELETE";
            if (method.GetCustomAttribute<HttpPutAttribute>() != null) return "PUT";

            // fallback to naming convention
            if (method.Name.StartsWith("Get", StringComparison.OrdinalIgnoreCase)) return "GET";
            if (method.Name.StartsWith("Delete", StringComparison.OrdinalIgnoreCase)) return "DELETE";
            if (useBody) return "POST";
            return "POST";
        }


        private static object? ConvertQuery(HttpContext ctx, ParameterInfo p)
        {
            var val = ctx.Request.Query[p.Name].FirstOrDefault();
            if (val == null) return p.ParameterType.IsValueType ? Activator.CreateInstance(p.ParameterType)! : null!;

            var targetType = p.ParameterType;
            var underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;

            try
            {
                // Enums
                if (underlyingType.IsEnum)
                    return Enum.Parse(underlyingType, val, ignoreCase: true);

                // GUIDs and TimeSpan
                if (underlyingType == typeof(Guid))
                    return Guid.Parse(val);
                if (underlyingType == typeof(TimeSpan))
                    return TimeSpan.Parse(val);

                // Type converter for common types (int, bool, datetime, etc.)
                var converter = System.ComponentModel.TypeDescriptor.GetConverter(underlyingType);
                if (converter != null && converter.CanConvertFrom(typeof(string)))
                    return converter.ConvertFromInvariantString(val);

                // Fallback to ChangeType with invariant culture
                return Convert.ChangeType(val, underlyingType, System.Globalization.CultureInfo.InvariantCulture);
            }
            catch
            {
                // If JSON can deserialize into complex reference types, try that
                try
                {
                    if (!underlyingType.IsValueType && underlyingType != typeof(string))
                    {
                        return System.Text.Json.JsonSerializer.Deserialize(val, underlyingType);
                    }
                }
                catch { }

               // As a last resort return default for value types, or the raw string for reference types
                return (underlyingType.IsValueType) ? Activator.CreateInstance(underlyingType)! : (object?)val;
            }
        }

        private static async Task<IResult> ToResult(object? result)
        {
            try
            {
                if (result is Task task)
                {
                    await task.ConfigureAwait(false);

                    // Handle Task<T>
                    var prop = task.GetType().GetProperty("Result");
                    var value = prop?.GetValue(task);

                    // Handle file download: (Stream, string fileName, string contentType)
                    var fileResult = TryGetFileResult(value);
                    if (fileResult != null) return fileResult;

                    return Results.Ok(new
                    {
                        Success = true,
                        Data = value,
                        Message = "Operation completed successfully"
                    });
                }

                // Non-task result
                return Results.Ok(new
                {
                    Success = true,
                    Data = result,
                    Message = "Operation completed successfully"
                });
            }
            catch (Exception ex)
            {
                // Wrap exception into a failure result
                return Results.BadRequest(new
                {
                    Success = false,
                    Data = (object?)null,
                    Message = ex.Message,
                    ExceptionType = ex.GetType().Name,
                    StackTrace = ex.StackTrace
                });
            }
        }

        private static IResult? TryGetFileResult(object? value)
        {
            if (value == null) return null;
            var type = value.GetType();
            if (!type.IsGenericType || !type.Name.StartsWith("ValueTuple")) return null;

            var fields = type.GetFields();
            var streamField = fields.FirstOrDefault(f => typeof(Stream).IsAssignableFrom(f.FieldType));
            if (streamField == null) return null;

            var stream = streamField.GetValue(value) as Stream;
            if (stream == null) return null;

            var stringFields = fields.Where(f => f.FieldType == typeof(string)).ToArray();
            var fileName = stringFields.Length > 0 ? stringFields[0].GetValue(value) as string ?? "download" : "download";
            var contentType = stringFields.Length > 1 ? stringFields[1].GetValue(value) as string ?? "application/octet-stream" : "application/octet-stream";

            return Results.File(stream, contentType, fileName);
        }

        private static Type GetUnwrappedReturnType(Type returnType)
        {
            // If the return type is a generic Task, return the type parameter
            if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
            {
                return returnType.GenericTypeArguments[0];
            }
            return returnType;
        }
    }
}
