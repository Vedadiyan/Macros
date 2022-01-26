namespace Macros.Net.Core.Annotations;

public class ParamAttribute : Attribute
{
    public string? Name { get; set; }
    public Type? OriginalType { get; set; }
}