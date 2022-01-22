namespace Macros.Inject;

public class Resolve
{
    public static T Service<T>(string? name = null)
    {
        return (T)Container.Current.Value.GetService((name ?? typeof(T).FullName)!).Value;
    }
    public static object Service(Type type, string? name = null)
    {
        return (T)Container.Current.Value.GetService((name ?? type.FullName)!).Value;
    }
}

