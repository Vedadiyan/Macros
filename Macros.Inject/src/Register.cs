using Macros.Inject.ServiceSpace;

namespace Macros.Inject;

public class Register
{
    public static void AddTransient<T, R>() where R : class, new()
    {
        Container.Current.Value.RegisterService(new TransientServiceSpace<R>(typeof(T).FullName!));
    }
    public static void AddTransient<T>(string name) where T : class, new()
    {
        Container.Current.Value.RegisterService(new TransientServiceSpace<T>(name));
    }
    public static void AddTransient<T>() where T : class, new()
    {
        Container.Current.Value.RegisterService(new TransientServiceSpace<T>(typeof(T).FullName!));
    }
    public static void AddTransient<T>(Func<T> instance) where T : class, new()
    {
        Container.Current.Value.RegisterService(new TransientServiceSpace<T>(instance, typeof(T).FullName!));
    }
    public static void AddTransient<T>(Func<T> instance, string name) where T : class, new()
    {
        Container.Current.Value.RegisterService(new TransientServiceSpace<T>(instance, name));
    }

    public static void AddScoped<T, R>() where R : class, new()
    {
        Container.Current.Value.RegisterService(new ScopedServiceSpace<R>(typeof(T).FullName!));
    }
    public static void AddScoped<T>(string name) where T : class, new()
    {
        Container.Current.Value.RegisterService(new ScopedServiceSpace<T>(name));
    }
    public static void AddScoped<T>() where T : class, new()
    {
        Container.Current.Value.RegisterService(new ScopedServiceSpace<T>(typeof(T).FullName!));
    }
    public static void AddScoped<T>(Func<T> instance) where T : class, new()
    {
        Container.Current.Value.RegisterService(new ScopedServiceSpace<T>(instance, typeof(T).FullName!));
    }
    public static void AddScoped<T>(Func<T> instance, string name) where T : class, new()
    {
        Container.Current.Value.RegisterService(new ScopedServiceSpace<T>(instance, name));
    }

    public static void AddSingleton<T, R>()
    {
        Container.Current.Value.RegisterService(new SingletonServiceSpace<R>(typeof(T).FullName!));
    }
    public static void AddSingleton<T>(string name)
    {
        Container.Current.Value.RegisterService(new SingletonServiceSpace<T>(name));
    }
        public static void AddSingleton<T>()
    {
        Container.Current.Value.RegisterService(new SingletonServiceSpace<T>(typeof(T).FullName!));
    }
    public static void AddSingleton<T>(T instance)
    {
        Container.Current.Value.RegisterService(new SingletonServiceSpace<T>(instance, typeof(T).FullName!));
    }
    public static void AddSingleton<T>(T instance, string name)
    {
        Container.Current.Value.RegisterService(new SingletonServiceSpace<T>(instance, name));
    }
}