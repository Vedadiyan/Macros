using System.Reflection;
using Macros.Inject;
using Macros.Net.Core.Abstraction;
using Macros.Net.Core.Abstraction.MacrosTransport;
using Macros.Net.Core.Annotations;
using Macros.Net.Core.Reflections;
using Macros.Progressives;

namespace Macros.Net.Core;

public sealed class ActionContext
{
    public string Route { get; }
    public IReadOnlyList<ParameterContext> Parameters { get; }
    public ControllerContext ControllerContext { get; }
    private readonly Func<object, object[], object> actionExecutor;
    private readonly MethodInfo methodInfo;
    private readonly IRouteGenerator routeGenerator;
    public ActionContext(ControllerContext controllerContext, MethodInfo methodInfo) :
    this(new Result<IRouteGenerator>(() => Resolve.Service<IRouteGenerator>()).Error(x => default!).Unwrap())
    {
        ControllerContext = controllerContext;
        this.methodInfo = methodInfo;
        Parameters = detectParamaters(methodInfo).ToList();
        if (methodInfo.TryGetCustomAttribute<RouteAttribute>(out RouteAttribute routeAttribute))
        {
            string route = routeGenerator.GenerateRoute(routeAttribute.RouteSegments);
            if (!routeGenerator.IsValidRoute(route))
            {
                throw new ArgumentException();
            }
            if (routeGenerator.IsRoot(route))
            {
                Route = route;
            }
            else
            {
                Route = routeGenerator.GenerateRoute(controllerContext.Route, route);
            }
        }
        else
        {
            Route = methodInfo.Name;
        }
        actionExecutor = methodInfo.Invoke!;
    }
    private ActionContext(IRouteGenerator routeGenerator)
    {
        this.routeGenerator = routeGenerator;
        this.methodInfo = null!;
        Route = null!;
        Parameters = null!;
        actionExecutor = null!;
    }
    public Func<object, object> GetActionExecutor(IMacrosTransport macrosTransport)
    {
        object[] parameters = Parameters.Select(x => x.GetValue(macrosTransport)).ToArray();
        return (object instance) => actionExecutor(instance, parameters);
    }
    public string? GetImplementationSpecificSetting(string implementation, string key)
    {
        IEnumerable<ForAttribute> forAttributes = methodInfo.GetCustomAttributes<ForAttribute>();
        return forAttributes.FirstOrDefault(x => x.Implementation.Equals(implementation, StringComparison.OrdinalIgnoreCase) && x.Key.Equals(key, StringComparison.OrdinalIgnoreCase))?.Value;
    }
    private static IEnumerable<ParameterContext> detectParamaters(MethodInfo methodInfo)
    {
        ParameterInfo[] parameterInfos = methodInfo.GetParameters();
        foreach (var parameterInfo in parameterInfos)
        {
            yield return new ParameterContext(parameterInfo, parameterInfo.GetCustomAttribute<ParamAttribute>()!);
        }
    }
}