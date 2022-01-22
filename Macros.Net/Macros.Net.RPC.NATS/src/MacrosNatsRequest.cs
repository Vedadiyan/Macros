using Macros.Net.RPC.Core.Abstraction.RPCProtocol;
using NATS.Client;

namespace Macros.Net.RPC.NATS;

public class MacrosNatsRequest : IMacrosRequest
{
    public string Namespace { get; }

    public string Action { get; }

    public IMacrosProtocol MacrosProtocol { get; }

    public MacrosNatsRequest(string @namespace, int namespaceSegments, Msg msg)
    {
        Namespace = @namespace;
        Action = string.Join(',', msg.Subject.Split('.').Skip(namespaceSegments - 1));
        MacrosProtocol = new MacrosNatsProtocol();
        MacrosProtocol.Deserialize(msg.Data);
        
    }

}