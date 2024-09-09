using Autofac;
using StarterKit.Maui.Common.Constants;
using StarterKit.Maui.Core.Data.Local;
using StarterKit.Maui.Core.Data.Remote;
using StarterKit.Maui.Features.Post.Data.Local;
using StarterKit.Maui.Features.Post.Data.Remote;
using StarterKit.Maui.Features.Post.Domain.Mappers;
using StarterKit.Maui.Features.Post.Domain.Models;
using StarterKit.Maui.Features.Post.Domain.Services;
using StarterKit.Maui.Features.Post.Presentation.ViewModels;
using StarterKit.Maui.Features.Post.Presentation.Views;
using StarterKit.Maui.Features.Startup.Presentation.ViewModels;
using StarterKit.Maui.Features.Startup.Presentation.Views;

namespace StarterKit.Maui.App.Container.Modules;

public class FeaturesModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);

        RegisterViews(builder);
        RegisterApis(builder);
        RegisterRepositories(builder);
        RegisterServices(builder);
        RegisterMappers(builder);
    }

    private static void RegisterViews(ContainerBuilder builder)
    {
        builder.RegisterView<SplashView, SplashViewModel>(ViewNames.Splash);
        builder.RegisterView<PostListView, PostListViewModel>(ViewNames.PostList);
    }

    private static void RegisterMappers(ContainerBuilder builder)
    {
        builder.RegisterType<PostMapper>().As<IPostMapper>().SingleInstance();
    }

    private static void RegisterServices(ContainerBuilder builder)
    {
        builder.RegisterType<PostService>().As<IPostService>().SingleInstance();
    }

    private static void RegisterApis(ContainerBuilder builder)
    {
        builder.Register(c => c.Resolve<HttpClientProvider>().GetApi<IPostApi>()).As<IPostApi>()
            .SingleInstance();
    }

    private static void RegisterRepositories(ContainerBuilder builder)
    {
        builder.RegisterType<PostRepository>().As<IRepository<PostEntity>>().SingleInstance();
    }
}

file static class ContainerBuilderExtensions
{
    public static void RegisterView<TView, TViewModel>(this ContainerBuilder builder, string viewName)
        where TView : notnull where TViewModel : notnull
    {
        builder.RegisterType<TView>().As<Page>().Named<Page>(viewName).InstancePerDependency();
        builder.RegisterType<TViewModel>().AsSelf().InstancePerDependency();
    }
}