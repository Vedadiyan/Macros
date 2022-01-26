namespace Macros.Net.Core.Abstraction.MacrosTransport;

public interface IMacrosResponse
{
    string Route { get; }
    int StatusCode { get; set;}
    void SetHeader(string name, string value);
    void SetResponse(object obj);
    ValueTask Respond();
}