namespace Macros.Net.Http;

public class MacrosHttpRouteRepository
{
    public static Lazy<MacrosHttpRouteRepository> Current { get; }
    private Dictionary<int, Dictionary<MacrosHttpMethods, List<MacrosHttpRoute>>> routes;
    static MacrosHttpRouteRepository()
    {
        Current = new Lazy<MacrosHttpRouteRepository>(() => new MacrosHttpRouteRepository(), true);
    }
    private MacrosHttpRouteRepository()
    {
        routes = new Dictionary<int, Dictionary<MacrosHttpMethods, List<MacrosHttpRoute>>>();
    }
    public void AddRoute(MacrosHttpRoute route)
    {
        if (routes.TryGetValue(route.Segments.Count, out Dictionary<MacrosHttpMethods, List<MacrosHttpRoute>>? routeGroup))
        {
            if (routeGroup.TryGetValue(route.HttpMethod, out List<MacrosHttpRoute>? routeList))
            {
                routeList.Add(route);
            }
            else
            {
                routeGroup.Add(route.HttpMethod, new List<MacrosHttpRoute> { route });
            }
        }
        else
        {
            routes.Add(route.Segments.Count, new Dictionary<MacrosHttpMethods, List<MacrosHttpRoute>>
            {
                [route.HttpMethod] = new List<MacrosHttpRoute> { route }
            });
        }
    }
    public IReadOnlyList<MacrosHttpRoute>? GetRoutes(int segments, MacrosHttpMethods macrosHttpMethods)
    {
        if (routes.TryGetValue(segments, out Dictionary<MacrosHttpMethods, List<MacrosHttpRoute>>? routeGroup))
        {
            if (routeGroup.TryGetValue(macrosHttpMethods, out List<MacrosHttpRoute>? routeList))
            {
                return routeList;
            }
        }
        return null;
    }
}