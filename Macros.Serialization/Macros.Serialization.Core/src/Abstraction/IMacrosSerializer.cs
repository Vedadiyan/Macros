namespace Macros.Serialization.Core.Abstraction;

public interface IMacrosSerializer {
    Stream Serialize<T>(T obj);
    T Deserialize<T>(byte[] bytes);
    object Deserialize(string value, Type type);
}