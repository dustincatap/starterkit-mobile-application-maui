using Microsoft.Extensions.Logging;
using StarterKit.Maui.Core.Domain.Exceptions;
using StarterKit.Maui.Core.Infrastructure;
using StarterKit.Maui.Core.Presentation.ViewModels;
using StarterKit.Maui.Core.Presentation.Views;

namespace StarterKit.Maui.Core.Presentation.Navigation;

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
		try
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
		catch (Exception ex)
		{
			NavigationException navEx = new NavigationException($"Failed to push {viewName}", ex);
			_logger.LogError(navEx, "Failed to push {ViewName}", viewName);

			throw navEx;
		}
	}

	public async Task PushToNewRoot(string viewName)
	{
		await PushToNewRoot(viewName, null);
	}

	public async Task PushToNewRoot(string viewName, object? parameter)
	{
		try
		{
			if (Navigation.NavigationStack.Count == 0)
			{
				await Push(viewName, parameter);
				return;
			}

			Page page = GetPage(viewName);
			Page currentRootPage = Navigation.NavigationStack[0];

			Navigation.InsertPageBefore(page, currentRootPage);
			await PopToRoot();
			TryInitializePage(page, parameter);

			Pushed?.Invoke(this, viewName);
			_logger.LogInformation("Pushed to new root {ViewName}", viewName);
		}
		catch (Exception ex)
		{
			NavigationException navEx = new NavigationException($"Failed to push to new root {viewName}", ex);
			_logger.LogError(navEx, "Failed to push to new root {ViewName}", viewName);

			throw navEx;
		}
	}

	public async Task Pop()
	{
		await Pop(null);
	}

	public async Task Pop(object? result)
	{
		try
		{
			string viewNameToPop = CurrentViewName;
			Page? poppedPage = await Navigation.PopAsync();

			if (poppedPage is null)
			{
				_logger.LogWarning("Failed to pop {ViewName}", viewNameToPop);
				return;
			}

			HandlePoppedPage(poppedPage);
			_logger.LogInformation("Popped {ViewName}", viewNameToPop);
		}
		catch (Exception ex)
		{
			NavigationException navEx = new NavigationException($"Failed to pop {CurrentViewName}", ex);
			_logger.LogError(navEx, "Failed to pop {ViewName}", CurrentViewName);

			throw navEx;
		}
	}

	public async Task PopToRoot()
	{
		try
		{
			// Get all pages except the root page
			List<Page> pagesToPop = Navigation.NavigationStack.Skip(1).ToList();

			await Navigation.PopToRootAsync();
			pagesToPop.ForEach(HandlePoppedPage);
			_logger.LogInformation("Popped to root {RootViewName}", CurrentViewName);
		}
		catch (Exception ex)
		{
			NavigationException navEx = new NavigationException("Failed to pop to root", ex);
			_logger.LogError(navEx, "Failed to pop to root");

			throw navEx;
		}
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
		try
		{
			Page page = ServiceLocator.GetService<Page>(viewName);
			ViewAttachedProperties.SetName(page, viewName);

			return page;
		}
		catch (Exception ex)
		{
			throw new InvalidOperationException($"Could not get page for {viewName}", ex);
		}
	}

	private void TryInitializePage(Page page, object? parameter)
	{
		if (page.BindingContext is not IInitialize initialize)
		{
			return;
		}

		try
		{
			MainThread.BeginInvokeOnMainThread(() => initialize.OnInitialize(parameter));
		}
		catch (Exception ex)
		{
			// Silently catch exceptions thrown during initialization
			_logger.LogError(ex, "Failed to initialize {ViewName}", ViewAttachedProperties.GetName(page));
		}
	}

	private void TryCleanupPage(Page page)
	{
		if (page.BindingContext is not ICleanup cleanup)
		{
			return;
		}

		try
		{
			MainThread.BeginInvokeOnMainThread(() => cleanup.OnCleanup());
		}
		catch (Exception ex)
		{
			// Silently catch exceptions thrown during cleanup
			_logger.LogError(ex, "Failed to cleanup {ViewName}", ViewAttachedProperties.GetName(page));
		}
	}
}
