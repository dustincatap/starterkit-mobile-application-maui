namespace StarterKit.Maui.Core.Infrastructure;

public static class ServiceLocator
{
	private static IServiceProvider? ServiceProvider;

	public static void Initialize(IServiceProvider serviceProvider)
	{
		ServiceProvider = serviceProvider;
	}

	public static T GetService<T>(string? name = null) where T : class
	{
		if (ServiceProvider is null)
		{
			throw new InvalidOperationException("Service provider not initialized.");
		}

		T service = string.IsNullOrWhiteSpace(name)
			? ServiceProvider.GetRequiredService<T>()
			: ServiceProvider.GetRequiredKeyedService<T>(name);

		return service;
	}
}
