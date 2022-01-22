using Macros.Net.RPC.Core.Abstraction.RPCProtocol;
using NATS.Client;

namespace Macros.Net.RPC.NATS;

public class MacrosNatsTransport : IMacrosTransport
{
    public IMacrosRequest Request { get; }

    public IMacrosResponse Response { get; }
    public MacrosNatsTransport(string @namespace, int namespaceSegments, Msg msg)
    {
        Request = new MacrosNatsRequest(@namespace, namespaceSegments, msg);
        Response = new MacrosNatsResponse(msg);
    }
}