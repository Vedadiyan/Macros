namespace Macros.Net.RPC.Core.Abstraction.RPCProtocol;

public interface IMacrosRequest
{
    string Namespace { get; }
    string Action { get; }
    IMacrosProtocol MacrosProtocol { get; }
}