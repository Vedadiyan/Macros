using System.Text.Json;

namespace Macros.Net.RPC.NATS;

public readonly struct NatsParameter
{
    public string Name { get; }
    public string Value { get; }
    public NatsParameter(string parameter)
    {
        string[] parameters = parameter.Split(':', 2);
        if(parameter.Length < 2) {
            throw new ArgumentException();
        }
        Name = parameters[0];
        Value = parameters[1];
    }
}