using Microsoft.Extensions.Logging;
using SQLite;
using StarterKit.Maui.Core.Data.Local;
using StarterKit.Maui.Features.Post.Domain.Models;

namespace StarterKit.Maui.App.Database;

public class AppDatabase : IAppDatabase
{
	private readonly ILogger<AppDatabase> _logger;
	private readonly SQLiteAsyncConnection _connection;

	public AppDatabase(ILogger<AppDatabase> logger, IDbConnectionFactory dbConnectionFactory)
	{
		_logger = logger;
		_connection = dbConnectionFactory.CreateConnection();
		_connection.CreateTableAsync<PostEntity>();
	}

	public TResult TryExecute<TResult>(Func<SQLiteAsyncConnection, TResult> action)
	{
		try
		{
			return action.Invoke(_connection);
		}
		catch (SQLiteException sqx)
		{
			_logger.LogError(sqx, "An error occurred while executing the db action");
			throw;
		}
	}
}
