namespace StarterKit.Maui.Core.Infrastructure.Resource;

public interface IEmbeddedResourceReader
{
	T? ReadAs<T>(string name, Type assemblyClass);
}
