using Microsoft.Extensions.Logging;
using StarterKit.Maui.Common.Constants;
using StarterKit.Maui.Common.Utilities;
using StarterKit.Maui.Core.Presentation.Navigation;
using StarterKit.Maui.Core.Presentation.ViewModels;

namespace StarterKit.Maui.Features.Startup.Presentation.ViewModels;

public class SplashViewModel : IInitialize
{
    private readonly ILogger<SplashViewModel> _logger;
    private readonly INavigationService _navigationService;
    private readonly ITaskUtils _taskUtils;

    public SplashViewModel(ILogger<SplashViewModel> logger,
        INavigationService navigationService,
        ITaskUtils taskUtils)
    {
        _logger = logger;
        _navigationService = navigationService;
        _taskUtils = taskUtils;
    }

    public async Task OnInitialize(object? parameter = null)
    {
        _logger.LogInformation("Initializing splash screen");

        await _taskUtils.Delay(2000);

        await _navigationService.PushToNewRoot(ViewNames.PostList);
    }
}