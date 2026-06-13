using System.Reflection;

namespace ENyayPath.PICS.Web.Helpers
{
    public static class WebModule
    {
        public static void AddDynamicApis(this IServiceCollection services, Assembly appAssembly)
        {
            // Auto-register all *AppService classes with their interfaces
            var appServiceTypes = appAssembly
                .GetTypes()
                .Where(t => t.Name.EndsWith("AppService") && t.IsClass && !t.IsAbstract);

            foreach (var implType in appServiceTypes)
            {
                var iface = implType.GetInterfaces()
                    .FirstOrDefault(i => i.Name == $"I{implType.Name}");
                if (iface != null)
                {
                    services.AddScoped(iface, implType);
                }
            }
        }

        public static void MapDynamicApis(this WebApplication app, Assembly appAssembly)
        {
            IEnumerable<Type> appServiceTypes;
            try { appServiceTypes = appAssembly.GetTypes(); }
            catch { appServiceTypes = Enumerable.Empty<Type>(); }

            appServiceTypes = appServiceTypes.Where(t => t.Name.EndsWith("AppService") && t.IsClass && !t.IsAbstract);

            var mappedRoutes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var implType in appServiceTypes)
            {
                var iface = implType.GetInterfaces()
                    .FirstOrDefault(i => i.Name == $"I{implType.Name}");
                if (iface == null) continue;

                var methods = iface.GetMethods();
                foreach (var method in methods)
                {
                    var route = $"/api/{implType.Name.Replace("AppService", "").ToLower()}/{method.Name.ToLower()}";
                    var parameters = method.GetParameters();

                    // Decide binding strategy: simple types via query, complex via JSON body
                    bool useBody = parameters.Length == 1 && !IsSimple(parameters[0].ParameterType);

                    if (useBody)
                    {
                        if (mappedRoutes.Contains(route)) continue;
                        mappedRoutes.Add(route);

                        app.MapPost(route, async (HttpContext ctx, IServiceProvider sp) =>
                        {
                            var service = sp.GetRequiredService(iface);
                            var dto = await ctx.Request.ReadFromJsonAsync(parameters[0].ParameterType);
                            var result = method.Invoke(service, new[] { dto! });

                            if (result is Task task)
                            {
                                await task;
                                var prop = task.GetType().GetProperty("Result");
                                return Results.Ok(prop?.GetValue(task));
                            }
                            return Results.Ok(result);
                        });
                    }
                    else
                    {
                        if (mappedRoutes.Contains(route)) continue;
                        mappedRoutes.Add(route);

                        app.MapGet(route, async (HttpContext ctx, IServiceProvider sp) =>
                        {
                            var service = sp.GetRequiredService(iface);
                            var args = parameters.Select(p =>
                                Convert.ChangeType(ctx.Request.Query[p.Name], p.ParameterType)).ToArray();
                            var result = method.Invoke(service, args);

                            if (result is Task task)
                            {
                                await task;
                                var prop = task.GetType().GetProperty("Result");
                                return Results.Ok(prop?.GetValue(task));
                            }
                            return Results.Ok(result);
                        });
                    }
                }
            }
        }

        private static bool IsSimple(Type type) =>
            type.IsPrimitive || type == typeof(string) || type == typeof(DateTime) || type == typeof(decimal);
    }
}
