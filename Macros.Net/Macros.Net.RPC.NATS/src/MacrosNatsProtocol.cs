using System.Text.Json;
using Macros.Net.RPC.Core.Abstraction.RPCProtocol;

namespace Macros.Net.RPC.NATS;

public class MacrosNatsProtocol : IMacrosProtocol
{
    public string Procedure { get; }

    public int Timeout { get; }

    public IReadOnlyDictionary<string, string> Headers { get; }

    public IReadOnlyDictionary<string, object> Params { get; }

    private byte[]? data;

    public MacrosNatsProtocol(byte[] data)
    {
        JsonDocument jsonDocument = JsonSerializer.Deserialize<JsonDocument>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        jsonDocument.RootElement.TryGetProperty("procedure", out JsonElement procedure);
        Procedure = procedure.GetString() ?? throw new InvalidDataException("Procedure is not present");
        jsonDocument.RootElement.TryGetProperty("timeout", out JsonElement timeout);
        Timeout = timeout.GetInt32();
        jsonDocument.RootElement.TryGetProperty("headers", out JsonElement headers);
        Headers = headers.Deserialize<Dictionary<string, string>>() ?? new Dictionary<string, string>();
        jsonDocument.RootElement.TryGetProperty("params", out JsonElement @params);
        Params = @params.Deserialize<Dictionary<string, object>>() ?? new Dictionary<string, object>();
        jsonDocument.RootElement.TryGetProperty("data", out JsonElement message);
        this.data = Convert.FromBase64String(message.GetString() ?? throw new InvalidDataException("Data is not present"));
    }

    public ReadOnlySpan<byte> GetData()
    {
        return data;
    }

    public void SetData(byte[] bytes)
    {
        data = bytes;
    }
}