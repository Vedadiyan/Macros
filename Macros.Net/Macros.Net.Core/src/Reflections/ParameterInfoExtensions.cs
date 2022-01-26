using System.Reflection;

namespace Macros.Net.Core.Reflections;

public static class ParameterInfoExtensions
{
    public static bool TryGetCustomAttribute<TType>(this ParameterInfo parameterInfo, out TType attribute) where TType : Attribute
    {
        attribute = parameterInfo.GetCustomAttribute<TType>()!;
        return attribute != null;
    }
    
}