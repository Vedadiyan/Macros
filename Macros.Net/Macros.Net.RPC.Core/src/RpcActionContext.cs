using System.Reflection;
using Macros.Net.RPC.Core.Abstraction.RPCProtocol;
using Macros.Net.RPC.Core.Annotations;
using Macros.Net.RPC.Core.Reflections;

namespace Macros.Net.RPC.Core;

public class RpcActionContext
{
    public string Name { get; }
    public IReadOnlyList<RpcParameterContext> Parameters { get; }
    private readonly Func<object, object[], object> actionExecutor;
    public RpcActionContext(MethodInfo methodInfo)
    {
        Parameters = detectParamaters(methodInfo).ToList();
        if (methodInfo.TryGetCustomAttribute<ActionAttribute>(out ActionAttribute actionAttribute))
        {
            if (actionAttribute.Name?.StartsWith('/') == true)
            {
                Name = actionAttribute.Name.TrimStart('/');
            }
            else
            {
                Name = actionAttribute.Name ?? methodInfo.Name;
            }
        }
        else
        {
            Name = methodInfo.Name;
        }
        actionExecutor = methodInfo.Invoke!;
    }
    public Func<object, object> GetActionExecutor(IMacrosTransport macrosTransport)
    {
        object[] paramertes = Parameters.Select(x => x.GetValue(macrosTransport)).ToArray();
        return (object instance) => actionExecutor(instance, paramertes);
    }
    private static IEnumerable<RpcParameterContext> detectParamaters(MethodInfo methodInfo)
    {
        ParameterInfo[] parameterInfos = methodInfo.GetParameters();
        foreach (var parameterInfo in parameterInfos)
        {
            yield return new RpcParameterContext(parameterInfo);
        }
    }
}