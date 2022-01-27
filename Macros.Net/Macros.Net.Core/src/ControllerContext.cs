using System.Reflection;
using Macros.Inject;
using Macros.Inject.Annotations;
using Macros.Net.Core.Abstraction;
using Macros.Net.Core.Abstraction.MacrosTransport;
using Macros.Net.Core.Annotations;
using Macros.Progressives;

namespace Macros.Net.Core;

public sealed class ControllerContext
{
    public string Route { get; }
    private readonly Dictionary<string, ActionContext> actions;
    private readonly Type type;
    private readonly IRouteGenerator routeGenerator;
    public ControllerContext(Type type, MethodInfo[] methodInfos) : 
    this(new Result<IRouteGenerator>(() => Resolve.Service<IRouteGenerator>()).Error(x => default!).Unwrap())
    {
        this.type = type;
        RouteAttribute? routeAttribute = type.GetCustomAttribute<RouteAttribute>();
        string route;
        if (routeAttribute != null)
        {
            route = routeGenerator.GenerateRoute(routeAttribute.RouteSegments);
        }
        else
        {
            route = type.FullName!;
        }
        if (!routeGenerator.IsValidRoute(route))
        {
            throw new ArgumentException();
        }
        Route = route;
        InjectableAttribute? injectableAttribute = type.GetCustomAttribute<InjectableAttribute>();
        if (injectableAttribute == null)
        {
            Macros.Inject.Register.AddTransient(type);
        }
        foreach (var methodInfo in methodInfos)
        {
            ActionAttribute actionAttribute = methodInfo.GetCustomAttribute<ActionAttribute>()!;
            actions.Add(actionAttribute?.Name ?? methodInfo.Name, new ActionContext(this, methodInfo));
        }
    }
    private ControllerContext(IRouteGenerator routeGenerator)
    {
        this.routeGenerator = routeGenerator;
        actions = new Dictionary<string, ActionContext>();
        type = null!;
        Route = null!;
    }
    public async void HandleRequest(IMacrosTransport macrosTransport)
    {
        if (actions.TryGetValue(macrosTransport.Request.Action, out ActionContext? actionContext))
        {
            try
            {
                object obj = actionContext.GetActionExecutor(macrosTransport)(Macros.Inject.Resolve.Service(type));
                if (obj is Task task)
                {
                    await task;
                }
                else if (obj is ValueTask valueTask)
                {
                    await valueTask;
                }
                if (macrosTransport.Response.StatusCode == 0)
                {
                    macrosTransport.Response.StatusCode = (int)MacrosStatusCodes.Ok;
                }
                macrosTransport.Response.SetResponse(obj);
                await macrosTransport.Response.Respond();
            }
            catch (Exception ex)
            {
                macrosTransport.Response.StatusCode = (int)MacrosStatusCodes.UnandledExpection;
                macrosTransport.Response.SetResponse(ex.Message);
                await macrosTransport.Response.Respond();
            }
        }
        else
        {
            macrosTransport.Response.StatusCode = (int)MacrosStatusCodes.ActionNotFound;
            macrosTransport.Response.SetResponse("Action not found");
            await macrosTransport.Response.Respond();
        }
    }
}