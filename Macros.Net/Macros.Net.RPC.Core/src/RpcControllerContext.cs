using System.Reflection;
using Macros.Net.RPC.Core.Abstraction.RPCProtocol;

namespace Macros.Net.RPC.Core;

public sealed class RpcControllerContext
{
    public string Namespace { get; }
    private readonly Dictionary<string, RpcActionContext> actions;
    private readonly Type type;
    public RpcControllerContext(Type type, string @namespace, MethodInfo[] methodInfos) {
        this.type = type;
        Namespace = @namespace;
        actions = new Dictionary<string, RpcActionContext>();
    }
    public async void HandleRequest(IMacrosTransport macrosTransport) {
        if(actions.TryGetValue(macrosTransport.Request.Action, out RpcActionContext? rpcActionContext)) {
            object obj = rpcActionContext.GetActionExecutor(macrosTransport)(Macros.Inject.Resolve.Service(type));
            if(obj is Task task) {
                await task;
            }
            else if (obj is ValueTask valueTask) {
                await valueTask;
            }
            macrosTransport.Response.SetResponse(obj);
        }
    }
}