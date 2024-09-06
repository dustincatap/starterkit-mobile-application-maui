using Autofac;
using StarterKit.Maui.Common.Utilities;

namespace StarterKit.Maui.App.Container.Modules;

public class CommonModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);

        RegisterUtilities(builder);
    }

    private static void RegisterUtilities(ContainerBuilder builder)
    {
        builder.RegisterType<TaskUtils>().As<ITaskUtils>().SingleInstance();
    }
}