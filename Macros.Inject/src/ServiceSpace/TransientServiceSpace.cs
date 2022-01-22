using Macros.Inject.Abstraction;

namespace Macros.Inject.ServiceSpace;

public class TransientServiceSpace<T> : IServiceSpace where T: class, new()
{
    public string ServiceName { get; }
    public Type ServiceType { get; }
    private readonly Func<T>? instance;

    public TransientServiceSpace(string serviceName)
    {
        ServiceType = typeof(T);
        ServiceName = serviceName;
    }

    public TransientServiceSpace(Func<T> instance, string serviceName)
    {
        ServiceType = typeof(T);
        ServiceName = serviceName;
        this.instance = instance;
    }

    public Lazy<object> InstanceGenerator()
    {
        return new Lazy<object>(() => instance?.Invoke() ?? this.CreateInstance(), true);
    }
}