using System.Reflection;
using Macros.Net.Core.Abstraction.MacrosTransport;
using Macros.Net.Core.Annotations;

namespace Macros.Net.Core;

public class ParameterContext
{
    public string Name { get; }
    private Type type;
    public ParameterContext(ParameterInfo parameterInfo, ParamAttribute requestParamAttribute)
    {
        Name = requestParamAttribute?.Name ?? parameterInfo.Name!;
        type = requestParamAttribute?.OriginalType ?? parameterInfo.ParameterType;
    }
    public object GetValue(IMacrosTransport macrosTransport)
    {
        if (type == typeof(IMacrosTransport))
        {
            return macrosTransport;
        }
        if (macrosTransport.Request.TryGetParam(type, Name, out object value))
        {
            return value;
        }
        return null!;
    }
}