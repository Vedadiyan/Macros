namespace Macros.Net.RPC.Core.Abstraction.RPCProtocol;

public interface IMacrosResponse
{
    string Namespace { get; }
    string Procedure { get; }
    IReadOnlyDictionary<string, string> Headers { get; }
    Task RepondeAsync(ReadOnlySpan<byte> bytes);
}