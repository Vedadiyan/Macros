namespace Macros.Net.Core.Abstraction;

public interface IMacrosServer {
    ValueTask StartAsync(CancellationToken cancellationToken);
}