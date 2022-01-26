namespace Macros.Net.RPC.Core.Abstraction.RPCProtocol;

public interface IMacrosResponseProtocol
{
    int StatusCode { get; }
    byte[] Data { get; }
}