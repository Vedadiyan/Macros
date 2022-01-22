using System.Text.Json;
using Macros.Net.RPC.Core.Abstraction.RPCProtocol;

namespace Macros.Net.RPC.NATS;

public class MacrosNatsProtocol : IMacrosProtocol
{
    public string Procedure { get; set; }

    public int Timeout { get; set; }

    public IReadOnlyDictionary<string, string> Headers { get; set; }

    public IReadOnlyDictionary<string, object> Params { get; set; }

    private byte[]? data;
    public ReadOnlySpan<byte> GetData()
    {
        return data;
    }

    public void SetData(byte[] bytes)
    {
        data = bytes;
    }
}