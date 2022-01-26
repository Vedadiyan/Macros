namespace Macros.Net.Core.Abstraction.MacrosTransport;

public interface IMacrosTransport
{
    IMacrosRequest Request { get; }
    IMacrosResponse Response { get; }
}