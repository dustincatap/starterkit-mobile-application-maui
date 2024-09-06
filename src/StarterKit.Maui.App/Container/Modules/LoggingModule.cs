using Autofac;
using Microsoft.Extensions.Logging;

namespace StarterKit.Maui.App.Container.Modules;

public class LoggingModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);

        builder.RegisterType<LoggerFactory>().As<ILoggerFactory>().SingleInstance();
        builder.RegisterGeneric(typeof(Logger<>)).As(typeof(ILogger<>)).SingleInstance();
    }
}