namespace StarterKit.Maui.Core.Infrastructure.Navigation;

public interface INavigationService
{
    event EventHandler<string> Pushed;

    event EventHandler<string> Popped;

    string CurrentViewName { get; }
    
    Task PushToNewRoot(string viewName);
    
    Task PushToNewRoot(string viewName, object? parameter);

    Task Push(string viewName, object? parameter = null);

    Task<T?> Push<T>(string viewName, object? parameter = null) where T : class;

    Task Pop();

    Task Pop(object? result);
    
    Task PopToRoot();
}