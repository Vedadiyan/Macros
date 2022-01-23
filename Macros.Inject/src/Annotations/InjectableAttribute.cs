namespace Macros.Inject.Annotations;

[AttributeUsage(AttributeTargets.Class)]
public class InjectableAttribute : Attribute
{
    public InjectionTypes InjectionType { get; }
    public string? Name { get; set; }
    public InjectableAttribute(InjectionTypes injectionType)
    {
        InjectionType = injectionType;
    }
}

public enum InjectionTypes
{
    Singleton,
    Transient,
    Scoped
}