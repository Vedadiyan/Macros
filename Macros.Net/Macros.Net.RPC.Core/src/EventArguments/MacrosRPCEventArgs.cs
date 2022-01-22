using Macros.Net.RPC.Core.Abstraction.RPCProtocol;

namespace Macros.Net.RPC.Core.EventArguments;

public class MacrosRPCEventArgs : EventArgs
{
    public IMacrosTransport MacrosTransport { get; }
    public MacrosRPCEventArgs(IMacrosTransport macrosTransport)
    {
        MacrosTransport = macrosTransport;
    }
}