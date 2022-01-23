using Macros.Inject.Abstraction;

namespace Macros.Inject.ServiceSpace;

public class TransientServiceSpace<T> : TransientServiceSpace where T : class, new()
{
    public TransientServiceSpace(string serviceName) : base(typeof(T), serviceName)
    {
    }
    public TransientServiceSpace(Func<object> instance, string serviceName) : base(typeof(T), instance, serviceName)
    {

    }
}
public class TransientServiceSpace : IServiceSpace
{
    public string ServiceName { get; }
    public Type ServiceType { get; }
    private readonly Func<object>? instance;

    public TransientServiceSpace(Type type, string serviceName)
    {
        ServiceType = type;
        ServiceName = serviceName;
    }

    public TransientServiceSpace(Type type, Func<object> instance, string serviceName)
    {
        ServiceType = type;
        ServiceName = serviceName;
        this.instance = instance;
    }

    public Lazy<object> InstanceGenerator()
    {
        return new Lazy<object>(() => instance?.Invoke() ?? this.CreateInstance(), true);
    }
}