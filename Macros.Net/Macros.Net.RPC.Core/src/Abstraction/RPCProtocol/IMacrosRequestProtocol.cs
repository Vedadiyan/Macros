namespace Macros.Net.RPC.Core.Abstraction.RPCProtocol;

public interface IMacrosRequestProtocol
{
    int Timeout { get; }
    IReadOnlyDictionary<string, string> Headers { get; }
    IReadOnlyDictionary<string, object> Params { get; }
    byte[] Data { get; }
}