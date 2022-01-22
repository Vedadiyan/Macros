namespace Macros.Inject.Abstraction;

public interface IServiceSpace
{
    string ServiceName { get; }
    Type ServiceType { get; }
    Lazy<object> InstanceGenerator();
    void Register()
    {
        if (Resolve() == null)
        {
            Container.Current.Value.RegisterService(this);
        }
    }
    Lazy<object> Resolve()
    {
        return Container.Current.Value.GetService(ServiceName);
    }
}