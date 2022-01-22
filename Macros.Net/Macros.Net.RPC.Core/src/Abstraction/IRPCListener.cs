using Macros.Net.RPC.Core.EventArguments;

namespace Macros.Net.RPC.Core.Abstraction;

public interface IRPCListener
{
    event EventHandler<MacrosRPCEventArgs> Next;
    Task ConnectAsync(CancellationToken cancellationToken);
}