using System.Reflection;
using Macros.Net.Core.Annotations;

namespace Macros.Net.Core.Reflections;

public static class AssemblyExtensions
{
    public static IEnumerable<Func<Func<string, string>, char, ControllerContext>> GetControllers(this Assembly assembly)
    {
        List<AssemblyName> assemblyNames = assembly.GetReferencedAssemblies().ToList();
        assemblyNames.Add(assembly.GetName());
        foreach (var assemblyName in assemblyNames)
        {
            Assembly referencedAssembly = Assembly.Load(assemblyName);
            Type[] exportedTypes = referencedAssembly.GetExportedTypes();
            foreach (var exportedType in exportedTypes)
            {
                if (exportedType.TryGetCustomAttribute<ControllerAttribute>(out ControllerAttribute controllerAttribute))
                {
                    MethodInfo[] methodInfos = exportedType.GetMethods().Where(x => x.GetCustomAttribute<NonActionAttribute>() == null).ToArray();
                    yield return (routeBuilder, c) => new ControllerContext(exportedType, methodInfos);
                }
            }
        }
    }
}