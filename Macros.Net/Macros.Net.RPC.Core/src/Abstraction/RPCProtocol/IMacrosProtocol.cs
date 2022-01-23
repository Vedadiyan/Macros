using System.Text.Json;

namespace Macros.Net.RPC.Core.Abstraction.RPCProtocol;

public interface IMacrosProtocol
{
    string Procedure { get; set; }
    int Timeout { get; set; }
    IReadOnlyDictionary<string, string> Headers { get; set; }
    IReadOnlyDictionary<string, Func<Type, object>> Params { get; set; }
    ReadOnlySpan<byte> GetData();
    void SetData(byte[] bytes);
    IMacrosProtocol Deserialize(byte[] bytes)
    {
        JsonDocument jsonDocument = JsonSerializer.Deserialize<JsonDocument>(bytes, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        if (jsonDocument.RootElement.TryGetProperty("procedure", out JsonElement procedure))
        {
            Procedure = procedure.GetString() ?? "";
        }
        if (jsonDocument.RootElement.TryGetProperty("timeout", out JsonElement timeout))
        {
            Timeout = timeout.GetInt32();
        }
        if (jsonDocument.RootElement.TryGetProperty("headers", out JsonElement headers))
        {
            Headers = headers.Deserialize<Dictionary<string, string>>() ?? new Dictionary<string, string>();
        }
        if (jsonDocument.RootElement.TryGetProperty("params", out JsonElement @params))
        {
            Params = @params.Deserialize<Dictionary<string, JsonElement>>()?.ToDictionary(x => x.Key, x => (Type type) => x.Value.Deserialize(type)) ?? new Dictionary<string, Func<Type, object>>();
        }
        if (jsonDocument.RootElement.TryGetProperty("data", out JsonElement message))
        {
            SetData(Convert.FromBase64String(message.GetString() ?? throw new InvalidDataException("Data is not present")));
        }
        return this;
    }
}