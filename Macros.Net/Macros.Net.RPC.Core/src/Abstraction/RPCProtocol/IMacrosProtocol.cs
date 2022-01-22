namespace Macros.Net.RPC.Core.Abstraction.RPCProtocol;

public interface IMacrosProtocol
{
    string Procedure { get; }
    int Timeout { get; }
    IReadOnlyDictionary<string, string> Headers { get; }
    IReadOnlyDictionary<string, object> Params { get; }
    ReadOnlySpan<byte> GetData();
    void SetData(byte[] bytes);
}