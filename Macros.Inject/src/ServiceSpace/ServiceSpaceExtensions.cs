using Macros.Inject.Abstraction;

namespace Macros.Inject.ServiceSpace;

public static class ServiceSpaceExtensions
{
    public static object CreateInstance(this IServiceSpace serviceSpace)
    {
        if (serviceSpace.ServiceType.GetConstructors().Where(x => x.GetParameters().Length == 0).Any())
        {
            return Activator.CreateInstance(serviceSpace.ServiceType) ?? throw new ArgumentException();
        }
        throw new ArgumentException("No public parameterless constructor found.");
    }
}