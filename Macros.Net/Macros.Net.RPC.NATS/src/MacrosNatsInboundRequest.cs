using System.Net;
using Macros.Net.RPC.Core.Abstraction.RPCProtocol;
using Macros.Serialization.Core.Abstraction;
using NATS.Client;

namespace Macros.Net.RPC.NATS;

public class MacrosNatsInboundRequest : IMacrosRequest
{
    public string Namespace { get; }
    public string Action { get; }
    private Dictionary<string, NatsParameter> parameters;
    private Dictionary<string, string> headers;
    private MemoryStream memoryStream;
    public MacrosNatsInboundRequest(string @namespace, int namespaceSegments, Msg msg)
    {
        Namespace = @namespace;
        Action = string.Join(',', msg.Subject.Split('.').Skip(namespaceSegments - 1));
        parameters = new Dictionary<string, NatsParameter>();
        headers = new Dictionary<string, string>();
        var natsParameters = msg.Header.GetValues("Param")?.Select(x => new NatsParameter(x));
        if (natsParameters != null)
        {
            foreach (var parameter in natsParameters)
            {
                parameters.Add(parameter.Name, parameter);
            }
        }
        foreach (string header in msg.Header.Keys)
        {
            headers.Add(header, string.Join(',', msg.Header.GetValues(header)));
        }
        memoryStream = new MemoryStream(msg.Data);
    }

    public string GetHeader(string name)
    {
        return headers[name];
    }

    public bool TryGetHeader(string name, out string value)
    {
        return headers.TryGetValue(name, out value!);
    }

    public string[] AvailableHeaders()
    {
        return headers.Keys.ToArray();
    }

    public string[] AvailableParams()
    {
        return parameters.Keys.ToArray();
    }

    public IPAddress GetRemoteClient()
    {
        throw new NotImplementedException();
    }

    public object GetParam(Type type, string name)
    {
        return Inject.Resolve.Service<IMacrosSerializer>(MacrosNatsServer.DefaultSerializerId).Deserialize(parameters[name].Value, type);
    }

    public bool TryGetParam(Type type, string name, out object value)
    {
        if (parameters.TryGetValue(name, out NatsParameter natsParameter))
        {
            value = Inject.Resolve.Service<IMacrosSerializer>(MacrosNatsServer.DefaultSerializerId).Deserialize(natsParameter.Value, type); ;
            return true;
        }
        value = null!;
        return false;
    }

    public Stream GetInputStream()
    {
        return memoryStream;
    }

    public void Dispose()
    {
        memoryStream.Dispose();
    }
}