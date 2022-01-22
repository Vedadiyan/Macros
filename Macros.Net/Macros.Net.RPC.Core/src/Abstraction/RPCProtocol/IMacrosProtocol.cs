using System.Text.Json;

namespace Macros.Net.RPC.Core.Abstraction.RPCProtocol;

public interface IMacrosProtocol
{
    string Procedure { get; set; }
    int Timeout { get; set; }
    IReadOnlyDictionary<string, string> Headers { get; set; }
    IReadOnlyDictionary<string, object> Params { get; set; }
    ReadOnlySpan<byte> GetData();
    void SetData(byte[] bytes);
    byte[] Serialize()
    {
        string serialized = JsonSerializer.Serialize(new
        {
            Procedure,
            Timeout,
            Headers,
            Params,
            Data = Convert.ToBase64String(GetData().ToArray())
        });
        return System.Text.Encoding.UTF8.GetBytes(serialized);
    }
    IMacrosProtocol Deserialize(byte[] bytes)
    {
        JsonDocument jsonDocument = JsonSerializer.Deserialize<JsonDocument>(bytes, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        jsonDocument.RootElement.TryGetProperty("procedure", out JsonElement procedure);
        Procedure = procedure.GetString() ?? throw new InvalidDataException("Procedure is not present");
        jsonDocument.RootElement.TryGetProperty("timeout", out JsonElement timeout);
        Timeout = timeout.GetInt32();
        jsonDocument.RootElement.TryGetProperty("headers", out JsonElement headers);
        Headers = headers.Deserialize<Dictionary<string, string>>() ?? new Dictionary<string, string>();
        jsonDocument.RootElement.TryGetProperty("params", out JsonElement @params);
        Params = @params.Deserialize<Dictionary<string, object>>() ?? new Dictionary<string, object>();
        jsonDocument.RootElement.TryGetProperty("data", out JsonElement message);
        SetData(Convert.FromBase64String(message.GetString() ?? throw new InvalidDataException("Data is not present")));
        return this;
    }
}