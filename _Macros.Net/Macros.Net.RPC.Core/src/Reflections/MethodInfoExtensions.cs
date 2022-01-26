using System.Reflection;

namespace Macros.Net.RPC.Core.Reflections;

public static class MethodInfoExtensions
{
    public static bool TryGetCustomAttribute<TType>(this MethodInfo methodInfo, out TType attribute) where TType : Attribute
    {
        attribute = methodInfo.GetCustomAttribute<TType>()!;
        return attribute != null;
    }
}