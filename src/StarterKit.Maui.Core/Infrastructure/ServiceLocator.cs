using System.Diagnostics.CodeAnalysis;

namespace StarterKit.Maui.Core.Infrastructure;

[ExcludeFromCodeCoverage]
public static class ServiceLocator
{
    public static IServiceProvider? ServiceProvider { get; set; }

    public static T GetService<T>(string? name = null) where T : class
    {
        T? service = string.IsNullOrWhiteSpace(name)
            ? ServiceProvider?.GetRequiredService<T>()
            : ServiceProvider?.GetKeyedService<T>(name);

        return service ?? throw new InvalidOperationException($"Service of type {typeof(T).Name} not found.");
    }
}