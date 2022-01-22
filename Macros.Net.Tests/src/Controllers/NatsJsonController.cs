using Macros.Net.RPC.Core.Annotations;

namespace Macros.Net.Tests.Controllers;

[RpcController]
public class NatsJsonController {
    public string Simple(string message) {
        return message;
    }
}