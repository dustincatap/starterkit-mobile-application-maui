using StarterKit.Maui.Core.Domain.Models;

namespace StarterKit.Maui.Core.Presentation.Navigation;

public interface INavigationService
{
    event EventHandler<string> Pushed;

    event EventHandler<string> Popped;

    string CurrentViewName { get; }

    Task<Result> PushToNewRoot(string viewName);

    Task<Result> PushToNewRoot(string viewName, object? parameter);

    Task<Result> Push(string viewName, object? parameter = null);

    Task<Result<T?>> Push<T>(string viewName, object? parameter = null) where T : class;

    Task<Result> Pop();

    Task<Result> Pop(object? result);

    Task<Result> PopToRoot();
}