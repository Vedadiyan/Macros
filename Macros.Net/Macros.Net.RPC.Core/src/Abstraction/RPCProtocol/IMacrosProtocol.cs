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
            procedure = Procedure,
            timeout = Timeout,
            headers = Headers,
            @params = Params,
            data = Convert.ToBase64String(GetData().ToArray())
        });
        return System.Text.Encoding.UTF8.GetBytes(serialized);
    }
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
            Params = @params.Deserialize<Dictionary<string, JsonElement>>()?.Select(x =>
            {
                switch (x.Value.ValueKind)
                {
                    case JsonValueKind.String:
                        {
                            return (x.Key, (object)x.Value.GetString()!);
                        }
                    case JsonValueKind.False:
                    case JsonValueKind.True:
                        {
                            return (x.Key, (object)x.Value.GetBoolean()!);
                        }
                    case JsonValueKind.Number:
                        {
                            decimal value = x.Value.GetDecimal();
                            return (x.Key, (value % 1 == 0 ? (value > int.MaxValue ? (object)(long)value : (object)(int)value) : (object)value));
                        }
                    case JsonValueKind.Null:
                        {
                            return (x.Key, null!);
                        }
                    default:
                        {
                            return (x.Key, null!);
                        }
                }
            }).ToDictionary(x => x.Key, x => x.Item2) ?? new Dictionary<string, object>();
        }
        if (jsonDocument.RootElement.TryGetProperty("data", out JsonElement message))
        {
            SetData(Convert.FromBase64String(message.GetString() ?? throw new InvalidDataException("Data is not present")));
        }
        return this;
    }
}