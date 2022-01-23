using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Macros.Net.RPC.Core.Abstraction.RPCProtocol;
using Macros.Net.RPC.NATS;
using Macros.Net.Tests.Controllers;
using NATS.Client;
using Xunit;

namespace Macros.Net.Tests.Tests;

public class NatsJsonControllerTest
{
    public NatsJsonControllerTest()
    {
        MacrosNatsServer macrosNatsServer = new MacrosNatsServer();
        macrosNatsServer.StartAsync(CancellationToken.None);
    }
    [Fact]
    public void SimpleTest()
    {
        var macrosNatsProtocolRequest = new
        {
            @params = new Dictionary<string, object>()
            {
                ["message"] = 1
            }
        };
        IConnection connection = new ConnectionFactory().CreateConnection();
        Msg msg = connection.Request("Test.Simple", System.Text.Encoding.UTF8.GetBytes(JsonSerializer.Serialize(macrosNatsProtocolRequest)));
        string data = System.Text.Encoding.UTF8.GetString(msg.Data);
    }
}