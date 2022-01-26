using System.Reflection;

namespace Macros.Net.Core.Reflections;

public static class TypeExtensions
{
    public static bool TryGetCustomAttribute<TType>(this Type type, out TType attribute) where TType : Attribute
    {
        attribute = type.GetCustomAttribute<TType>()!;
        return attribute != null;
    }
}