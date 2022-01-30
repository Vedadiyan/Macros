namespace Macros.Net.Core.Annotations;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class ForAttribute : Attribute
{
    public string Implementation { get; }
    public string Key { get; }
    public string Value { get; }
    public ForAttribute(string implementation, string key, string value) {
        Implementation = implementation;
        Key = key;
        Value = value;
    }
}