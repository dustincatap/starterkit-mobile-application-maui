using SQLite;

namespace StarterKit.Maui.Core.Data.Local;

public interface IDbConnectionFactory
{
	SQLiteAsyncConnection CreateConnection();
}
