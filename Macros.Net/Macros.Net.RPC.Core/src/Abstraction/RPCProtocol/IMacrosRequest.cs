namespace Macros.Net.RPC.Core.Abstraction.RPCProtocol;

public interface IMacrosRequest
{
    string Namespace { get; }
    string Action { get; }
    string Procedure { get; }
    int Timeout { get; }
    IReadOnlyDictionary<string, string> Headers { get; }
    IReadOnlyDictionary<string, object> Params { get; }
    ReadOnlySpan<byte> Data { get; }
    TType ReadDataAs<TType>();
}