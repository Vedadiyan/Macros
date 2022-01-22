namespace Macros.Net.RPC.Core.Abstraction;

public interface IMacrosServer {
    Task StartAsync(CancellationToken cancellationToken);
}