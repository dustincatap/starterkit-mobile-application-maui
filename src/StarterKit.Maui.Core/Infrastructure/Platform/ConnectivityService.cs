namespace StarterKit.Maui.Core.Infrastructure.Platform;

public class ConnectivityService : IConnectivityService
{
	public bool IsInternetConnected => Connectivity.Current.NetworkAccess == NetworkAccess.Internet;
}
