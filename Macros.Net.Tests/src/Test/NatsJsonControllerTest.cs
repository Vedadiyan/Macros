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
        Macros.Inject.Register.AddTransient<NatsJsonController>();
    }
    [Fact]
    public void SimpleTest()
    {
        IMacrosProtocol macrosNatsProtocolRequest = new MacrosNatsProtocol
        {
            Params = new Dictionary<string, object>()
            {
                ["message"] = 1
            }
        };
        IConnection connection = new ConnectionFactory().CreateConnection();
        Msg msg = connection.Request("Test.Simple", macrosNatsProtocolRequest.Serialize());
        IMacrosProtocol macrosNatsProtocolResponse = new MacrosNatsProtocol();
        macrosNatsProtocolResponse.Deserialize(msg.Data);
        string response = JsonSerializer.Deserialize<string>(macrosNatsProtocolResponse.GetData())!;
        Assert.Equal(response, "1");
    }
}