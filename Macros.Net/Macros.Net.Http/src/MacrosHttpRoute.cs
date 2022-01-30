using System.Diagnostics.CodeAnalysis;
using Macros.Net.Core;

namespace Macros.Net.Http;

public readonly struct MacrosHttpRoute
{
    public ActionContext ActionContext { get; }
    public List<KeyValuePair<string, bool>> Segments { get; }
    public MacrosHttpMethods HttpMethod { get; }
    public MacrosHttpRoute(ActionContext actionContext)
    {
        ActionContext = actionContext;
        string? httpMethod = actionContext.GetImplementationSpecificSetting("Http", "Method");
        HttpMethod = MacrosHttpMethods.GET;
        if (!string.IsNullOrEmpty(httpMethod))
        {
            HttpMethod = Enum.Parse<MacrosHttpMethods>(httpMethod.ToUpper());
        }
        Segments = new List<KeyValuePair<string, bool>>();
        foreach (var segment in actionContext.Route.Split('/'))
        {
            string tmp = segment.TrimStart().TrimEnd();
            if (tmp.StartsWith("{") && tmp.EndsWith("}"))
            {
                Segments.Add(new KeyValuePair<string, bool>(tmp.TrimStart('{').TrimEnd('}'), true));
            }
            else
            {
                Segments.Add(new KeyValuePair<string, bool>(tmp, false));
            }
        }
    }
    public int Match(string[] segments)
    {
        int count = 0;
        for (int i = 0; i < segments.Length; i++)
        {
            KeyValuePair<string, bool> routeSegment = Segments[i];
            if (!routeSegment.Value)
            {
                if (routeSegment.Key.Equals(segments[i], StringComparison.OrdinalIgnoreCase))
                {
                    count++;
                }
                else
                {
                    return -1;
                }
            }
        }
        return count;
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string? ToString()
    {
        return base.ToString();
    }

    public static bool operator ==(MacrosHttpRoute macrosHttpRouteA, MacrosHttpRoute macrosHttpRouteB)
    {
        return macrosHttpRouteA.GetHashCode() == macrosHttpRouteB.GetHashCode();
    }
    public static bool operator !=(MacrosHttpRoute macrosHttpRouteA, MacrosHttpRoute macrosHttpRouteB)
    {
        return macrosHttpRouteA.GetHashCode() != macrosHttpRouteB.GetHashCode();
    }
}