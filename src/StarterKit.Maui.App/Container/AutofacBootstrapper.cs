using Autofac;
using StarterKit.Maui.App.Container.Modules;

namespace StarterKit.Maui.App.Container;

public static class AutofacBootstrapper
{
	public static void RegisterAutofacModules(ContainerBuilder containerBuilder)
	{
		containerBuilder.RegisterModule(new CommonModule());
		containerBuilder.RegisterModule(new CoreModule());
		containerBuilder.RegisterModule(new FeaturesModule());
		containerBuilder.RegisterModule(new LoggingModule());
	}
}
