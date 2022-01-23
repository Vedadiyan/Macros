using Macros.Inject;
using Macros.Inject.Abstraction;

namespace Macros.Inject.ServiceSpace;

public class ScopedServiceSpace<T> : ScopedServiceSpace where T : class, new()
{
    public ScopedServiceSpace(string serviceName) : base(typeof(T), serviceName)
    {
    }
    public ScopedServiceSpace(Func<T> instance, string serviceName) : base(typeof(T), instance, serviceName)
    {
    }
}
public class ScopedServiceSpace : IServiceSpace
{
    private Dictionary<object, WeakReference<Lazy<object>>> instances;
    public string ServiceName { get; }
    public Type ServiceType { get; }
    private readonly Func<object>? instance;

    public ScopedServiceSpace(Type type, string serviceName)
    {
        ServiceType = type;
        ServiceName = serviceName;
        instances = new Dictionary<object, WeakReference<Lazy<object>>>();
    }
    public ScopedServiceSpace(Type type, Func<object> instance, string serviceName)
    {
        ServiceType = type;
        ServiceName = serviceName;
        this.instance = instance;
        instances = new Dictionary<object, WeakReference<Lazy<object>>>();
    }
    public Lazy<object> InstanceGenerator()
    {
        IScopeProvider? scopeProvider = Container.Current.Value.GetService(typeof(IScopeProvider).FullName!).Value as IScopeProvider;
        if (scopeProvider == null)
        {
            throw new InvalidOperationException("No instance of IScopeProvider available in the container.");
        }
        if (instances.TryGetValue(scopeProvider.Get(), out WeakReference<Lazy<object>>? weakInstance))
        {
            if (weakInstance.TryGetTarget(out Lazy<object>? instance))
            {
                return instance;
            }
            else
            {
                instance = new Lazy<object>(() => this.instance?.Invoke() ?? this.CreateInstance());
                instances[scopeProvider.Get()] = new WeakReference<Lazy<object>>(instance);
                return instance;
            }
        }
        else
        {
            Lazy<object> instance = new Lazy<object>(() => this.instance?.Invoke() ?? this.CreateInstance());
            weakInstance = new WeakReference<Lazy<object>>(instance);
            instances.Add(scopeProvider.Get(), weakInstance);
            return instance;
        }
    }
}
