using Macros.Net.Core.Abstraction.MacrosTransport;

namespace Macros.Net.Core.EventArguments;

public class MacrosListenerEventArgs : EventArgs
{
    public IMacrosTransport MacrosTransport { get; }
    public MacrosListenerEventArgs(IMacrosTransport macrosTransport)
    {
        MacrosTransport = macrosTransport;
    }
}