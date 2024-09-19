using SQLite;
using StarterKit.Maui.Core.Infrastructure.Environment;
using StarterKit.Maui.Core.Infrastructure.Platform;

namespace StarterKit.Maui.Core.Data.Local;

public class DbConnectionFactory : IDbConnectionFactory
{
	private readonly IEnvironmentVariables _environmentVariables;
	private readonly IPathProvider _pathProvider;

	public DbConnectionFactory(IEnvironmentVariables environmentVariables, IPathProvider pathProvider)
	{
		_environmentVariables = environmentVariables;
		_pathProvider = pathProvider;
	}

	public SQLiteAsyncConnection CreateConnection()
	{
		SQLiteConnectionString connStr = new SQLiteConnectionString(_pathProvider.DatabasePath,
			SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache, true,
			_environmentVariables.DatabaseCipherKey);
		SQLiteAsyncConnection connection = new SQLiteAsyncConnection(connStr);
		connection.ExecuteScalarAsync<string>($"PRAGMA key={_environmentVariables.DatabaseCipherKey}");

		return connection;
	}
}
