using System.Net;

namespace Macros.Net.Http;

public class MacrosHttpRouter
{
    public static MacrosHttpRoute FindBestRoute(HttpListenerRequest httpListenerRequest) {
        MacrosHttpRouteRepository macrosHttpRouteRepository = MacrosHttpRouteRepository.Current.Value;
        int segments = httpListenerRequest.Url?.Segments.Length ?? 0;
        MacrosHttpMethods macrosHttpMethods = Enum.Parse<MacrosHttpMethods>(httpListenerRequest.HttpMethod.ToUpper());
        IReadOnlyList<MacrosHttpRoute>? routes = macrosHttpRouteRepository.GetRoutes(segments, macrosHttpMethods);
        if(routes != null) {
            return routes.Where(x=> x.Match(httpListenerRequest.Url?.Segments ?? new string[0]) > -1).FirstOrDefault();
        }
        return default;
    }
}