namespace Macros.Net.RPC.Core.Abstraction.RPCProtocol;

public interface IMacrosResponse
{
    string Namespace { get; }
    int StatusCode { get; set;}
    void SetHeader(string name, string value);
    void SetResponse(object obj);
    ValueTask Respond();
}