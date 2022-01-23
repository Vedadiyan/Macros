using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Macros.Net.RPC.Core.Abstraction.RPCProtocol;
using Macros.Net.RPC.NATS;
using Macros.Net.Tests.Controllers;
using Macros.Serialization.Core.Abstraction;
using NATS.Client;
using Xunit;

namespace Macros.Net.Tests.Tests;

public class NatsJsonControllerTest
{
    public NatsJsonControllerTest()
    {
        MacrosNatsServer macrosNatsServer = new MacrosNatsServer(new DefaultSerializer());
        macrosNatsServer.StartAsync(CancellationToken.None);
    }
    [Fact]
    public void SimpleTest()
    {
        var xx = JsonSerializer.Serialize("1");
        var macrosNatsProtocolRequest = new
        {
            @params = new Dictionary<string, object>()
            {
                ["message"] = 1
            }
        };
        IConnection connection = new ConnectionFactory().CreateConnection();
        MsgHeader msgHeader = new MsgHeader();
        msgHeader.Add("Param", "message:1");
        Msg msg = connection.Request(new Msg("Test.Simple", msgHeader, new byte[] { }));
        string data = System.Text.Encoding.UTF8.GetString(msg.Data);
    }
}

public class DefaultSerializer : IMacrosSerializer
{
    public T Deserialize<T>(byte[] bytes)
    {
        return JsonSerializer.Deserialize<T>(bytes)!;
    }

    public object Deserialize(string value, Type type)
    {
        return JsonSerializer.Deserialize(value, type)!;
    }

    public Stream Serialize<T>(T obj)
    {
        return new MemoryStream(System.Text.Encoding.UTF8.GetBytes(JsonSerializer.Serialize(obj)));
    }
}