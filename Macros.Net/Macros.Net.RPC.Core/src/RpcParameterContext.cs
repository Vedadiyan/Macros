using System.Reflection;
using Macros.Net.RPC.Core.Abstraction.RPCProtocol;
using Macros.Net.RPC.Core.Annotations;

namespace Macros.Net.RPC.Core;

public class RpcParameterContext
{
    public string Name { get; }
    private Type type;
    public RpcParameterContext(ParameterInfo parameterInfo, RequestParamAttribute requestParamAttribute)
    {
        Name = requestParamAttribute?.Name ?? parameterInfo.Name!;
        type = requestParamAttribute?.OriginalType ?? parameterInfo.ParameterType;
    }
    public object GetValue(IMacrosTransport macrosTransport)
    {
        if (type == typeof(IMacrosTransport))
        {
            return macrosTransport;
        }
        if (macrosTransport.Request.MacrosProtocol.Params.TryGetValue(Name, out Func<Type, object>? value))
        {
            return value(type);
        }
        return null!;
    }
}