using Macros.Inject.Abstraction;

namespace Macros.Inject.ServiceSpace;

public class SingletonServiceSpace<T> : IServiceSpace
{
    private readonly Lazy<object> instance;
    public string ServiceName { get; }
    public Type ServiceType { get; }

    public SingletonServiceSpace(string serviceName)
    {
        ServiceType = typeof(T);
        ServiceName = serviceName;
        instance = new Lazy<object>(() => this.CreateInstance(), true);
    }
    public SingletonServiceSpace(T instance, string serviceName)
    {
        ServiceType = typeof(T);
        ServiceName = serviceName;
        this.instance = new Lazy<object>(() => instance ?? throw new NullReferenceException(), true);
    }

    public Lazy<object> InstanceGenerator()
    {
        return instance;
    }
}