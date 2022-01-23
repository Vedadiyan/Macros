using System.Reflection;
using Macros.Inject.Annotations;

namespace Macros.Inject.Relfections;

public static class AssemblyExtensions
{
    public static void RegisterInjectables(this Assembly assembly)
    {
        List<AssemblyName> assemblyNames = assembly.GetReferencedAssemblies().ToList();
        assemblyNames.Add(assembly.GetName());
        foreach (var assemblyName in assemblyNames)
        {
            IEnumerable<Type> exportedTypes = Assembly.Load(assemblyName).ExportedTypes;
            foreach (var exportedType in exportedTypes)
            {
                InjectableAttribute? injectableAttribute = exportedType.GetCustomAttribute<InjectableAttribute>();
                if (injectableAttribute != null)
                {
                    switch (injectableAttribute.InjectionType)
                    {
                        case InjectionTypes.Singleton:
                            {
                                Register.AddSingleton(exportedType, injectableAttribute.Name ?? exportedType.FullName!);
                                break;
                            }
                        case InjectionTypes.Transient:
                            {
                                Register.AddTransient(exportedType, injectableAttribute.Name ?? exportedType.FullName!);
                                break;
                            }
                        case InjectionTypes.Scoped:
                            {
                                Register.AddScoped(exportedType, injectableAttribute.Name ?? exportedType.FullName!);
                                break;
                            }
                    }
                }
            }
        }
    }
}