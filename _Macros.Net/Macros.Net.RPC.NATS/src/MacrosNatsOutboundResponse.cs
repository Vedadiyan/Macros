using System.Text.Json;
using Macros.Net.RPC.Core;
using Macros.Net.RPC.Core.Abstraction.RPCProtocol;
using NATS.Client;

namespace Macros.Net.RPC.NATS;

public class MacrosNatsOutboundResponse : IMacrosResponse
{
    public string Namespace { get; init; }
    public int StatusCode { get; set; }
    private readonly Msg msg;
    private byte[] data;
    private MsgHeader msgHeader;

    public MacrosNatsOutboundResponse(Msg msg)
    {
        this.msg = msg;
        Namespace = msg.Reply;
        data = null!;
        msgHeader = new MsgHeader();
    }

    public ValueTask Respond()
    {
        msgHeader.Set("StatusCode", StatusCode.ToString());
        msg.ArrivalSubscription.Connection.Publish(new Msg(Namespace, msgHeader, data));
        return ValueTask.CompletedTask;
    }

    public void SetResponse(object obj)
    {
        data = System.Text.Encoding.UTF8.GetBytes(JsonSerializer.Serialize(obj));
    }

    public void SetHeader(string name, string value)
    {
        throw new NotImplementedException();
    }
}