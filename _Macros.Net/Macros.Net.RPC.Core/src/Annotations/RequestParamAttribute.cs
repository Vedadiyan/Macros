namespace Macros.Net.RPC.Core.Annotations;

public class RequestParamAttribute : Attribute
{
    public string? Name { get; set; }
    public Type? OriginalType { get; set; }
}