using Macros.Net.RPC.Core.Abstraction.RPCProtocol;
using Macros.Net.RPC.Core.Annotations;

namespace Macros.Net.Tests.Controllers;

[RpcController(Namespace = "Test")]
public class NatsJsonController {
    public string Simple(int message, IMacrosTransport macrosTransport) {
        return message.ToString();
    }
}