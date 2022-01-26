using System.Reflection;
using Macros.Net.Core.Abstraction.MacrosTransport;
using Macros.Net.Core.Annotations;
using Macros.Net.Core.Reflections;

namespace Macros.Net.Core;

public class ActionContext
{
    public string Route { get; }
    public IReadOnlyList<ParameterContext> Parameters { get; }
    private readonly Func<object, object[], object> actionExecutor;
    public ActionContext(string controllerRoute,char routeDelimiter, MethodInfo methodInfo)
    {
        Parameters = detectParamaters(methodInfo).ToList();
        if (methodInfo.TryGetCustomAttribute<RouteAttribute>(out RouteAttribute routeAttribute))
        {
            if (routeAttribute.Route.StartsWith(routeDelimiter) == true)
            {
                Route = routeAttribute.Route.TrimStart(routeDelimiter);
            }
            else
            {
                Route = controllerRoute + routeDelimiter.ToString() + routeAttribute.Route;
            }
        }
        else
        {
            Route = methodInfo.Name;
        }
        actionExecutor = methodInfo.Invoke!;
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