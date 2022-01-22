namespace Macros.Net.RPC.Core.Abstraction;

public interface IMacrosServer {
    ValueTask StartAsync(CancellationToken cancellationToken);
}