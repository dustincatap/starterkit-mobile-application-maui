using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace StarterKit.Maui.Core.Infrastructure.Resource;

[ExcludeFromCodeCoverage]
public class EmbeddedResourceReader : IEmbeddedResourceReader
{
    public string ReadAsString(string name, Type assemblyClass)
    {
        try
        {
            using Stream? jsonStream = Assembly.GetAssembly(assemblyClass)?.GetManifestResourceStream($"{name}");

            if (jsonStream == null)
            {
                throw new InvalidOperationException($"Could not find {name} file");
            }

            using StreamReader streamReader = new StreamReader(jsonStream, Encoding.UTF8);
            string value = streamReader.ReadToEnd();

            return value;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Could not read {name} file", ex);
        }
    }

    public T? ReadAs<T>(string name, Type assemblyClass)
    {
        string value = ReadAsString(name, assemblyClass);
        T? deserializedValue = JsonSerializer.Deserialize<T>(value);
            
        return deserializedValue;
    }
}