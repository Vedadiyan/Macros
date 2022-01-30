using Macros.Net.Core.Annotations;

namespace Macros.Net.Core.Abstraction;

public interface IRouteGenerator {
    string GenerateRoute(params string[] routeSegments);
    bool IsRoot(string route);
    bool IsValidRoute(string route);
}