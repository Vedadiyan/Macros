using Macros.Net.RPC.Core.Abstraction.RPCProtocol;

namespace Macros.Net.RPS.NATS;

public class MacrosNatsTransport : IMacrosTransport
{
    public IMacrosRequest Request => throw new NotImplementedException();

    public IMacrosResponse Response => throw new NotImplementedException();
}