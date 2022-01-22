using System.Text.Json;
using Macros.Net.RPC.Core.Abstraction.RPCProtocol;

namespace Macros.Net.RPC.NATS;

public class MacrosNatsProtocol : IMacrosProtocol
{
    public string Procedure { get; set; } = string.Empty;

    public int Timeout { get; set; }

    public IReadOnlyDictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();

    public IReadOnlyDictionary<string, object> Params { get; set; } = new Dictionary<string, object>();

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