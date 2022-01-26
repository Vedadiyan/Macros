using System.Reflection;
using Macros.Net.RPC.Core.Annotations;

namespace Macros.Net.RPC.Core.Reflections;

public static class AssemblyExtensions
{
    public static IEnumerable<RpcControllerContext> GetControllers(this Assembly assembly)
    {
        List<AssemblyName> assemblyNames = assembly.GetReferencedAssemblies().ToList();
        assemblyNames.Add(assembly.GetName());
        foreach (var assemblyName in assemblyNames)
        {
            Assembly referencedAssembly = Assembly.Load(assemblyName);
            Type[] exportedTypes = referencedAssembly.GetExportedTypes();
            foreach (var exportedType in exportedTypes)
            {
                if (exportedType.TryGetCustomAttribute<RpcControllerAttribute>(out RpcControllerAttribute rpcControllerAttribute))
                {
                    MethodInfo[] methodInfos = exportedType.GetMethods().Where(x=> x.GetCustomAttribute<NonActionAttribute>() == null).ToArray();
                    yield return new RpcControllerContext(exportedType, (rpcControllerAttribute.Namespace ?? exportedType.FullName  ?? throw new NullReferenceException()) + ".*", methodInfos);
                }
            }
        }
    }
}