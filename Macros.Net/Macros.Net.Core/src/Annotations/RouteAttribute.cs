namespace Macros.Net.Core.Annotations;

public class RouteAttribute : Attribute
{
    public string[] RouteSegments { get; }
    public RouteAttribute(params string[] routeSegments)
    {
        this.RouteSegments = routeSegments;
    }
}