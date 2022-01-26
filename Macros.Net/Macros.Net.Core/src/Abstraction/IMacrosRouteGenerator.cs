namespace Macros.Net.Core.Abstraction;

public interface IMacrosRouteGenerator {
    string Format();
    string Add(string segment);
}