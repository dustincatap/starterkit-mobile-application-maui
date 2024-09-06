using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using StarterKit.Maui.Core.Presentation.ViewModels;
using StarterKit.Maui.Core.Presentation.Views;

namespace StarterKit.Maui.Core.Infrastructure.Navigation;

[ExcludeFromCodeCoverage]
public class NavigationService : INavigationService
{
    private static INavigation Navigation => Application.Current?.MainPage?.Navigation ??
                                             throw new InvalidOperationException("MainPage is not set");

    private readonly ILogger<NavigationService> _logger;
    private readonly Dictionary<string, TaskCompletionSource<object?>> _pendingPushTasks;

    public NavigationService(ILogger<NavigationService> logger)
    {
        _logger = logger;
        _pendingPushTasks = new Dictionary<string, TaskCompletionSource<object?>>();
    }

    public event EventHandler<string>? Pushed;

    public event EventHandler<string>? Popped;

    public string CurrentViewName
    {
        get
        {
            Page currentPage = Navigation.NavigationStack.ToList().Last();
            string? pageName = ViewAttachedProperties.GetName(currentPage);

            return pageName ?? throw new InvalidOperationException("Page name is not set");
        }
    }

    public async Task Push(string viewName, object? parameter = null)
    {
        await Push<object>(viewName, parameter);
    }

    public async Task<T?> Push<T>(string viewName, object? parameter = null) where T : class
    {
        Page page = GetPage(viewName);
        await Navigation.PushAsync(page);
        TryInitializePage(page, parameter);

        Pushed?.Invoke(this, viewName);
        _logger.LogInformation("Pushed to {ViewName}", viewName);

        TaskCompletionSource<object?> tcs = new TaskCompletionSource<object?>();
        _pendingPushTasks[page.Id.ToString()] = tcs;
        object? result = await tcs.Task;

        if (result is T typedResult)
        {
            return typedResult;
        }

        if (result is not null)
        {
            _logger.LogWarning("Expected result of type {ExpectedType} but received {ActualType}", typeof(T),
                result.GetType());
        }

        return null;
    }
    
    public async Task PushToNewRoot(string viewName)
    {
        await PushToNewRoot(viewName, null);
    }

    public async Task PushToNewRoot(string viewName, object? parameter)
    {
        Page page = GetPage(viewName);
        Page currentRootPage = Navigation.NavigationStack[0];
        
        Navigation.InsertPageBefore(page, currentRootPage);
        await PopToRoot();
        TryInitializePage(page, parameter);

        Pushed?.Invoke(this, viewName);
        _logger.LogInformation("Pushed to new root {ViewName}", viewName);
    }

    public async Task Pop()
    {
        await Pop(null);
    }

    public async Task Pop(object? result)
    {
        string viewNameToPop = CurrentViewName;
        Page? poppedPage = await Navigation.PopAsync();

        if (poppedPage is null)
        {
            _logger.LogWarning("Failed to pop {ViewName}", viewNameToPop);
            return;
        }

        HandlePoppedPage(poppedPage);
    }

    public async Task PopToRoot()
    {
        // Get all pages except the root page
        List<Page> pagesToPop = Navigation.NavigationStack.Skip(1).ToList();
        
        await Navigation.PopToRootAsync();
        
        pagesToPop.ForEach(HandlePoppedPage);

        _logger.LogInformation("Popped to root {RootViewName}", CurrentViewName);
    }

    private void HandlePoppedPage(Page poppedPage)
    {
        TryCleanupPage(poppedPage);

        string viewNameToPop = ViewAttachedProperties.GetName(poppedPage);
        Popped?.Invoke(this, viewNameToPop);
        _logger.LogInformation("Popped {ViewName}", viewNameToPop);

        string pageId = poppedPage.Id.ToString();
        _pendingPushTasks.TryGetValue(pageId, out TaskCompletionSource<object?>? tcs);
        tcs?.TrySetResult(null);
        _pendingPushTasks.Remove(pageId);
    }

    private static Page GetPage(string viewName)
    {
        Page page = ServiceLocator.GetService<Page>(viewName);
        ViewAttachedProperties.SetName(page, viewName);

        return page;
    }

    private static void TryInitializePage(Page page, object? parameter)
    {
        if (page.BindingContext is IInitialize initialize)
        {
            MainThread.BeginInvokeOnMainThread(() => initialize.OnInitialize(parameter));
        }
    }

    private static void TryCleanupPage(Page page)
    {
        if (page.BindingContext is ICleanup cleanup)
        {
            MainThread.BeginInvokeOnMainThread(() => cleanup.OnCleanup());
        }
    }
}