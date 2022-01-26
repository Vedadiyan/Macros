using System.Net;

namespace Macros.Net.Core.Abstraction.MacrosTransport;

public interface IMacrosRequest: IDisposable
{
    string Route { get; }
    string Action { get; }
    string GetHeader(string name);
    bool TryGetHeader(string name, out string value);
    string[] AvailableHeaders();
    object GetParam(Type type, string name);
    bool TryGetParam(Type type, string name, out object value);
    string[] AvailableParams();
    IPAddress GetRemoteClient();
    Stream GetInputStream();
}