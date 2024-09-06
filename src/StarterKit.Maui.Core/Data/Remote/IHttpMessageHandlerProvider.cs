namespace StarterKit.Maui.Core.Data.Remote;

public interface IHttpMessageHandlerProvider
{
    HttpMessageHandler GetHandler();
}