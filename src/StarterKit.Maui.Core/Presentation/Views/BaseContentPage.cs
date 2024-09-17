using System.Diagnostics.CodeAnalysis;
using StarterKit.Maui.Core.Infrastructure;
using StarterKit.Maui.Core.Presentation.Navigation;

namespace StarterKit.Maui.Core.Presentation.Views;

[ExcludeFromCodeCoverage]
public abstract class BaseContentPage<T> : ContentPage where T : class
{
    private readonly INavigationService _navigationService;

    protected BaseContentPage()
    {
        _navigationService = ServiceLocator.GetService<INavigationService>();

        BindingContext = ServiceLocator.GetService<T>();

        Shell.SetBackButtonBehavior(this, new BackButtonBehavior
        {
            Command = new Command(() => OnBackButtonPressed())
        });
    }

    protected override bool OnBackButtonPressed()
    {
        if (Navigation.NavigationStack.Count == 1)
        {
            return false;
        }

        _navigationService.Pop();

        return true;
    }
}