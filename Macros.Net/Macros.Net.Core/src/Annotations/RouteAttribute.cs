namespace Macros.Net.Core.Annotations;

public class RouteAttribute : Attribute
{
    public string Route { get; }
    public RouteAttribute(string route)
    {
        Route = route;
    }
}