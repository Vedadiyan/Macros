using System.Reflection;
using Macros.Net.RPC.Core.Abstraction.RPCProtocol;

namespace Macros.Net.RPC.Core;

public class RpcParameterContext
{
    public string Name { get; }
    public RpcParameterContext(ParameterInfo parameterInfo)
    {   
        Name = parameterInfo.Name ?? throw new NullReferenceException();

    }
    public object GetValue(IMacrosTransport macrosTransport) {
        if(macrosTransport.Request.MacrosProtocol.Params.TryGetValue(Name, out object? value)) {
            return value;
        }
        return null!;
    }
}