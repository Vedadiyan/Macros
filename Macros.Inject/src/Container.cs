using Macros.Inject.Abstraction;

namespace Macros.Inject;

internal class Container
{
    internal static Lazy<Container> Current { get; } = new Lazy<Container>(() => new Container(), true);
    private Dictionary<string, IServiceSpace> types;
    private Container()
    {
        types = new Dictionary<string, IServiceSpace>();
    }
    internal void RegisterService(IServiceSpace serviceSpace) {
        types.Add(serviceSpace.ServiceName, serviceSpace);
    }
    internal Lazy<object> GetService(string name)
    {
        if(types.TryGetValue(name, out IServiceSpace? serviceSpace)) {
            return serviceSpace.InstanceGenerator();
        }
        throw new ArgumentException("Service could not be found.");
    }
}