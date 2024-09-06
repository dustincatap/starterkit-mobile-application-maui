using StarterKit.Maui.Common.Constants;
using StarterKit.Maui.Core.Infrastructure;
using StarterKit.Maui.Core.Infrastructure.Navigation;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace StarterKit.Maui.App;

public partial class App
{
    public App()
    {
        InitializeComponent();
        
        INavigationService navigationService = ServiceLocator.GetService<INavigationService>();
 
        MainPage = new NavigationPage();

        navigationService.Push(ViewNames.Splash);
    }
}