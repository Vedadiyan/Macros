using Macros.Net.RPC.Core.Annotations;

namespace Macros.Net.Tests.Controllers;

[RpcController(Namespace = "Test")]
public class NatsJsonController {
    public string Simple(int message) {
        return message.ToString();
    }
}