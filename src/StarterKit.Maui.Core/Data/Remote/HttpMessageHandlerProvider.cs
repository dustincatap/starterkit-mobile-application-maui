using Microsoft.Extensions.Logging;
using StarterKit.Maui.Core.Infrastructure;

namespace StarterKit.Maui.Core.Data.Remote;

public class HttpMessageHandlerProvider : IHttpMessageHandlerProvider
{
    public HttpMessageHandler GetHandler()
    {
        List<DelegatingHandler> defaultHandlers =
        [
            new HttpLoggingHandler(ServiceLocator.GetService<ILogger<HttpLoggingHandler>>())
        ];

        DelegatingHandler arrangedHandler = GetAndArrangeHandlers(defaultHandlers);

        return arrangedHandler;
    }

    private static DelegatingHandler GetAndArrangeHandlers(IList<DelegatingHandler> handlers)
    {
        // loop and set InnerHandler of each item to the next item
        for (int i = 0; i <= handlers.Count - 1; i++)
        {
            HttpMessageHandler nextHandler = handlers.ElementAtOrDefault(i + 1) as HttpMessageHandler ??
                                             new HttpClientHandler();
            handlers[i].InnerHandler = nextHandler;
        }

        return handlers[0];
    }
}