using System.Diagnostics.CodeAnalysis;
using Refit;
using StarterKit.Maui.Core.Infrastructure.Environment;

namespace StarterKit.Maui.Core.Data.Remote;

[ExcludeFromCodeCoverage]
public class HttpClientProvider
{
    private readonly IHttpMessageHandlerProvider _httpMessageHandlerProvider;
    private readonly IEnvironmentVariables _environmentVariables;

    public HttpClientProvider(IHttpMessageHandlerProvider httpMessageHandlerProvider,
        IEnvironmentVariables environmentVariables)
    {
        _httpMessageHandlerProvider = httpMessageHandlerProvider;
        _environmentVariables = environmentVariables;
    }

    public T GetApi<T>()
    {
        RefitSettings refitSettings = new RefitSettings
        {
            ContentSerializer = new SystemTextJsonContentSerializer()
        };

        HttpClient httpClient = new HttpClient(_httpMessageHandlerProvider.GetHandler())
        {
            BaseAddress = new Uri(_environmentVariables.ApiBaseUrl),
            Timeout = TimeSpan.FromSeconds(30)
        };

        return RestService.For<T>(httpClient, refitSettings);
    }
}