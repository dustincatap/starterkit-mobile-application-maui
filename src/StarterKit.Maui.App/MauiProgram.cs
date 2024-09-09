using Autofac.Extensions.DependencyInjection;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using StarterKit.Maui.App.Container;
using StarterKit.Maui.Core.Infrastructure;

namespace StarterKit.Maui.App;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        MauiAppBuilder builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .ConfigureContainer(new AutofacServiceProviderFactory(AutofacBootstrapper.RegisterAutofacModules));

#if DEBUG
        builder.Logging.AddDebug();
#endif

        MauiApp app = builder.Build();
        ServiceLocator.ServiceProvider = app.Services;

        return app;
    }
}