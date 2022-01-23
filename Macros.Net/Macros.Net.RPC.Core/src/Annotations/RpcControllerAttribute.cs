namespace Macros.Net.RPC.Core.Annotations;

public class RpcControllerAttribute : Attribute
{
    public string? Namespace { get; set; }
}