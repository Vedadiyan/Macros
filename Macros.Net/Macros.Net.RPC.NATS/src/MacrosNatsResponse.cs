using System.Text.Json;
using Macros.Net.RPC.Core.Abstraction.RPCProtocol;
using NATS.Client;

namespace Macros.Net.RPC.NATS;

public class MacrosNatsResponse : IMacrosResponse
{
    public string Namespace { get; init; }
    private readonly Msg msg;
    public IMacrosProtocol MacrosProtocol { get; }
    public MacrosNatsResponse(Msg msg)
    {
        this.msg = msg;
        Namespace = msg.Reply;
        MacrosProtocol = new MacrosNatsProtocol();
    }

    public ValueTask Respond()
    {
        msg.Respond(MacrosProtocol.Serialize());
        return ValueTask.CompletedTask;
    }

    public void SetResponse(object obj)
    {
        MacrosProtocol.SetData(System.Text.Encoding.UTF8.GetBytes(JsonSerializer.Serialize(obj)));
    }
}