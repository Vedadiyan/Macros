using System.Reflection;
using Macros.Inject.Annotations;
using Macros.Net.RPC.Core.Abstraction.RPCProtocol;
using Macros.Net.RPC.Core.Annotations;

namespace Macros.Net.RPC.Core;

public sealed class RpcControllerContext
{
    public string Namespace { get; }
    private readonly Dictionary<string, RpcActionContext> actions;
    private readonly Type type;
    public RpcControllerContext(Type type, string @namespace, MethodInfo[] methodInfos)
    {
        this.type = type;
        Namespace = @namespace;
        actions = new Dictionary<string, RpcActionContext>();
        InjectableAttribute? injectableAttribute = type.GetCustomAttribute<InjectableAttribute>();
        if (injectableAttribute == null)
        {
            Macros.Inject.Register.AddTransient(type);
        }
        foreach (var methodInfo in methodInfos)
        {
            ActionAttribute actionAttribute = methodInfo.GetCustomAttribute<ActionAttribute>()!;
            actions.Add(actionAttribute?.Name ?? methodInfo.Name, new RpcActionContext(methodInfo));
        }
    }
    public async void HandleRequest(IMacrosTransport macrosTransport)
    {
        if (actions.TryGetValue(macrosTransport.Request.Action, out RpcActionContext? rpcActionContext))
        {
            try
            {
                object obj = rpcActionContext.GetActionExecutor(macrosTransport)(Macros.Inject.Resolve.Service(type));
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