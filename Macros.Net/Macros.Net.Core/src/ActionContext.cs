using System.Reflection;
using Macros.Inject;
using Macros.Net.Core.Abstraction;
using Macros.Net.Core.Abstraction.MacrosTransport;
using Macros.Net.Core.Annotations;
using Macros.Net.Core.Reflections;
using Macros.Progressives;

namespace Macros.Net.Core;

public class ActionContext
{
    public string Route { get; }
    public IReadOnlyList<ParameterContext> Parameters { get; }
    private readonly Func<object, object[], object> actionExecutor;
    private readonly IRouteGenerator routeGenerator;
    public ActionContext(ControllerContext controllerContext, MethodInfo methodInfo) :
    this(new Result<IRouteGenerator>(() => Resolve.Service<IRouteGenerator>()).Error(x => default!).Unwrap())
    {
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
        Route = null!;
        Parameters = null!;
        actionExecutor = null!;
    }
    public Func<object, object> GetActionExecutor(IMacrosTransport macrosTransport)
    {
        object[] paramertes = Parameters.Select(x => x.GetValue(macrosTransport)).ToArray();
        return (object instance) => actionExecutor(instance, paramertes);
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