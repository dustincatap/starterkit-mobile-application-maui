using Microsoft.Extensions.Logging;
using StarterKit.Maui.Core.Domain.Models;
using StarterKit.Maui.Core.Infrastructure;
using StarterKit.Maui.Core.Presentation.ViewModels;
using StarterKit.Maui.Core.Presentation.Views;
using System.Diagnostics.CodeAnalysis;

namespace StarterKit.Maui.Core.Presentation.Navigation;

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

	public async Task<Result> Push(string viewName, object? parameter = null)
	{
		Result<object?> result = await Push<object>(viewName, parameter);

		return result switch
		{
			Success<object?> => new Success(),
			Failure<object?> failure => new Failure(failure.Exception),
			_ => new Failure(new InvalidOperationException("Unexpected result type"))
		};
	}

	public async Task<Result<T?>> Push<T>(string viewName, object? parameter = null) where T : class
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
				return new Success<T?>(typedResult);
			}

			if (result is not null)
			{
				_logger.LogWarning("Expected result of type {ExpectedType} but received {ActualType}", typeof(T),
					result.GetType());
			}

			return new Success<T?>(null);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Failed to push {ViewName}", viewName);
			return new Failure<T?>(ex);
		}
	}

	public async Task<Result> PushToNewRoot(string viewName)
	{
		Result result = await PushToNewRoot(viewName, null);

		return result switch
		{
			Success => new Success(),
			Failure failure => new Failure(failure.Exception),
			_ => new Failure(new InvalidOperationException("Unexpected result type"))
		};
	}

	public async Task<Result> PushToNewRoot(string viewName, object? parameter)
	{
		try
		{
			if (Navigation.NavigationStack.Count == 0)
			{
				await Push(viewName, parameter);
				return new Success();
			}

			Page page = GetPage(viewName);
			Page currentRootPage = Navigation.NavigationStack[0];

			Navigation.InsertPageBefore(page, currentRootPage);
			await PopToRoot();
			TryInitializePage(page, parameter);

			Pushed?.Invoke(this, viewName);
			_logger.LogInformation("Pushed to new root {ViewName}", viewName);

			return new Success();
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Failed to push to new root {ViewName}", viewName);
			return new Failure(ex);
		}
	}

	public async Task<Result> Pop()
	{
		Result result = await Pop(null);

		return result switch
		{
			Success => new Success(),
			Failure failure => new Failure(failure.Exception),
			_ => new Failure(new InvalidOperationException("Unexpected result type"))
		};
	}

	public async Task<Result> Pop(object? result)
	{
		try
		{
			string viewNameToPop = CurrentViewName;
			Page? poppedPage = await Navigation.PopAsync();

			if (poppedPage is null)
			{
				_logger.LogWarning("Failed to pop {ViewName}", viewNameToPop);
				return new Failure(new InvalidOperationException("Failed to pop"));
			}

			HandlePoppedPage(poppedPage);
			_logger.LogInformation("Popped {ViewName}", viewNameToPop);

			return new Success();
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Failed to pop");
			return new Failure(ex);
		}
	}

	public async Task<Result> PopToRoot()
	{
		try
		{
			// Get all pages except the root page
			List<Page> pagesToPop = Navigation.NavigationStack.Skip(1).ToList();

			await Navigation.PopToRootAsync();
			pagesToPop.ForEach(HandlePoppedPage);
			_logger.LogInformation("Popped to root {RootViewName}", CurrentViewName);

			return new Success();
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Failed to pop to root");
			return new Failure(ex);
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
			initialize.OnInitialize(parameter);
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
			cleanup.OnCleanup();
		}
		catch (Exception ex)
		{
			// Silently catch exceptions thrown during cleanup
			_logger.LogError(ex, "Failed to cleanup {ViewName}", ViewAttachedProperties.GetName(page));
		}
	}
}
