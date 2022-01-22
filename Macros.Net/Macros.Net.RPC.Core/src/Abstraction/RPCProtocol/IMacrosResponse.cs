namespace Macros.Net.RPC.Core.Abstraction.RPCProtocol;

public interface IMacrosResponse
{
    string Namespace { get; }
    IMacrosProtocol MacrosProtocol { get; }
    void SetResponse(object obj);
    ValueTask Respond();
}