using Autofac;
using Microsoft.EntityFrameworkCore;
using StarterKit.Maui.App.Database;
using StarterKit.Maui.Core.Data.Remote;
using StarterKit.Maui.Core.Infrastructure.Environment;
using StarterKit.Maui.Core.Infrastructure.Platform;
using StarterKit.Maui.Core.Infrastructure.Resource;
using StarterKit.Maui.Core.Presentation.Dialogs;
using StarterKit.Maui.Core.Presentation.Navigation;

namespace StarterKit.Maui.App.Container.Modules;

public class CoreModule : Module
{
	protected override void Load(ContainerBuilder builder)
	{
		base.Load(builder);

		RegisterInfrastructure(builder);
		RegisterData(builder);
		RegisterPresentation(builder);
	}

	private static void RegisterData(ContainerBuilder builder)
	{
		builder.RegisterType<HttpMessageHandlerProvider>().As<IHttpMessageHandlerProvider>()
			.SingleInstance();

		builder.RegisterType<HttpClientProvider>().As<HttpClientProvider>().SingleInstance();

		builder.RegisterType<AppDbContext>().As<DbContext>().InstancePerDependency();
	}

	private static void RegisterInfrastructure(ContainerBuilder builder)
	{
		builder.RegisterType<EmbeddedResourceReader>().As<IEmbeddedResourceReader>().SingleInstance();

		builder.Register(c =>
			{
				EnvironmentFileReader envFileReader = new EnvironmentFileReader(c.Resolve<IEmbeddedResourceReader>());
				IEnvironmentVariables env = envFileReader.Read("configurations.json", typeof(CoreModule));
				return env;
			})
			.As<IEnvironmentVariables>()
			.SingleInstance();

		builder.RegisterType<PathProvider>().As<IPathProvider>().SingleInstance();
		builder.RegisterType<ConnectivityService>().As<IConnectivityService>().SingleInstance();
	}

	private static void RegisterPresentation(ContainerBuilder builder)
	{
		builder.RegisterType<NavigationService>().As<INavigationService>().SingleInstance();
		builder.RegisterType<DialogService>().As<IDialogService>().SingleInstance();
	}
}
