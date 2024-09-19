using SQLite;

namespace StarterKit.Maui.Core.Data.Local;

public interface IAppDatabase
{
	TResult TryExecute<TResult>(Func<SQLiteAsyncConnection, TResult> action);
}
