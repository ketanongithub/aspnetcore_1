using ENyayPath.PICS.Application.DTOs;
using ENyayPath.PICS.Application.Settings;
using System.Reflection;

namespace ENyayPath.PICS.Web.Helpers
{
    public static class DynamicApiRegistrar
    {
        public static void MapApplicationServices(WebApplication app, IServiceProvider sp, Assembly appAssembly)
        {
            var appServiceTypes = appAssembly
                .GetTypes()
                .Where(t => t.Name.EndsWith("AppService") && t.IsClass && !t.IsAbstract);

            foreach (var implType in appServiceTypes)
            {
                var iface = implType.GetInterfaces()
                    .FirstOrDefault(i => i.Name == $"I{implType.Name}");
                if (iface == null) continue;

                var methods = iface.GetMethods();
                foreach (var method in methods)
                {
                    var parameters = method.GetParameters();

                    // Build route: /api/{service}/{method}
                    var route = $"/api/{implType.Name.Replace("AppService", "").ToLower()}/{method.Name.ToLower()}";

                    // Decide verb
                    var verb = GetHttpVerb(method.Name);

                    // Decide binding strategy
                    bool useBody = parameters.Length == 1 && !IsSimple(parameters[0].ParameterType);

                    if (verb == "GET" && !useBody)
                    {
                        app.MapGet(route, async (HttpContext ctx, IServiceProvider sp) =>
                        {
                            var service = sp.GetRequiredService(iface);
                            var args = parameters.Select(p =>
                                ConvertQueryValue(ctx.Request.Query[p.Name], p.ParameterType)).ToArray();
                            var result = method.Invoke(service, args);
                            return await ToResultAsync(result);
                        });
                    }
                    else if (verb == "POST" && useBody)
                    {
                        app.MapPost(route, async (HttpContext ctx, IServiceProvider sp) =>
                        {
                            var service = sp.GetRequiredService(iface);
                            var dto = await ctx.Request.ReadFromJsonAsync(parameters[0].ParameterType);
                            var result = method.Invoke(service, new[] { dto! });
                            return await ToResultAsync(result);
                        });
                    }
                    else if (verb == "DELETE")
                    {
                        app.MapDelete(route, async (HttpContext ctx, IServiceProvider sp) =>
                        {
                            var service = sp.GetRequiredService(iface);
                            var args = parameters.Select(p =>
                                ConvertQueryValue(ctx.Request.Query[p.Name], p.ParameterType)).ToArray();
                            var result = method.Invoke(service, args);
                            return await ToResultAsync(result);
                        });
                    }
                    else
                    {
                        // fallback POST with query args
                        app.MapPost(route, async (HttpContext ctx, IServiceProvider sp) =>
                        {
                            var service = sp.GetRequiredService(iface);
                            var args = parameters.Select(p =>
                                ConvertQueryValue(ctx.Request.Query[p.Name], p.ParameterType)).ToArray();
                            var result = method.Invoke(service, args);
                            return await ToResultAsync(result);
                        });
                    }
                }
            }
        }

        private static string GetHttpVerb(string methodName)
        {
            if (methodName.StartsWith("Get")) return "GET";
            if (methodName.StartsWith("Create") || methodName.StartsWith("Set") || methodName.StartsWith("Update")) return "POST";
            if (methodName.StartsWith("Delete")) return "DELETE";
            return "POST";
        }

        private static bool IsSimple(Type type) =>
            type.IsPrimitive || type == typeof(string) || type == typeof(DateTime) || type == typeof(decimal);

        private static object? ConvertQueryValue(string? value, Type targetType)
        {
            if (value == null) return targetType.IsValueType ? Activator.CreateInstance(targetType) : null;
            if (targetType == typeof(string)) return value;
            if (targetType == typeof(int) && int.TryParse(value, out var i)) return i;
            if (targetType == typeof(long) && long.TryParse(value, out var l)) return l;
            if (targetType == typeof(bool) && bool.TryParse(value, out var b)) return b;
            if (targetType == typeof(DateTime) && DateTime.TryParse(value, out var dt)) return dt;
            if (targetType == typeof(decimal) && decimal.TryParse(value, out var d)) return d;
            return System.ComponentModel.TypeDescriptor.GetConverter(targetType).ConvertFromInvariantString(value);
        }

        private static async Task<IResult> ToResultAsync(object? result)
        {
            if (result is Task task)
            {
                await task;
                var prop = task.GetType().GetProperty("Result");
                return Results.Ok(prop?.GetValue(task));
            }
            return Results.Ok(result);
        }
    }
}