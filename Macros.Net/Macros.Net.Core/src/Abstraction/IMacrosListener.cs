using Macros.Net.Core.EventArguments;

namespace Macros.Net.Core.Abstraction;

public interface IMacrosListener
{
    event EventHandler<MacrosListenerEventArgs> Next;
    Task ConnectAsync(CancellationToken cancellationToken);
}