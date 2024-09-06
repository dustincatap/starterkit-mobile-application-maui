namespace StarterKit.Maui.Core.Infrastructure.Resource;

public interface IEmbeddedResourceReader
{
    string ReadAsString(string name, Type assemblyClass);
        
    T? ReadAs<T>(string name, Type assemblyClass);
}