using Macros.Inject.Abstraction;

namespace Macros.Inject.ServiceSpace;

public class SingletonServiceSpace<T> : SingletonServiceSpace
{
    public SingletonServiceSpace(string serviceName) : base(typeof(T), serviceName)
    {
    }
    public SingletonServiceSpace(T instance, string serviceName): base(typeof(T), instance!, serviceName) {

    }
}

public class SingletonServiceSpace : IServiceSpace
{
    private readonly Lazy<object> instance;
    public string ServiceName { get; }
    public Type ServiceType { get; }

    public SingletonServiceSpace(Type type, string serviceName)
    {
        ServiceType = type;
        ServiceName = serviceName;
        instance = new Lazy<object>(() => this.CreateInstance(), true);
    }
    public SingletonServiceSpace(Type type, object instance, string serviceName)
    {
        ServiceType = type;
        ServiceName = serviceName;
        this.instance = new Lazy<object>(() => instance ?? throw new NullReferenceException(), true);
    }

    public Lazy<object> InstanceGenerator()
    {
        return instance;
    }
}