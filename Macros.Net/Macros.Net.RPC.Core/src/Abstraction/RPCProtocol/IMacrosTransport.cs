namespace Macros.Net.RPC.Core.Abstraction.RPCProtocol;

public interface IMacrosTransport
{
    IMacrosRequest Request { get; }
    IMacrosResponse Response { get; }
}