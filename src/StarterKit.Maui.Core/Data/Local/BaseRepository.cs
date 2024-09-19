using SQLite;
using StarterKit.Maui.Core.Domain.Models;

namespace StarterKit.Maui.Core.Data.Local;

public abstract class BaseRepository<T> : IRepository<T> where T : class, IEntity, new()
{
	private readonly IAppDatabase _database;

	protected BaseRepository(IAppDatabase database)
	{
		_database = database;
	}

	public async Task<T?> Get(Predicate<T> filter)
	{
		T? entity = await TryExecute(conn => conn.Table<T>().Where(x => filter(x)).FirstOrDefaultAsync());
		return entity;
	}

	public async Task<IEnumerable<T>> GetAll()
	{
		IEnumerable<T> entities = await TryExecute(dbSet => dbSet.Table<T>().ToListAsync());
		return entities;
	}

	public async Task<IEnumerable<T>> GetAll(Predicate<T> filter)
	{
		IEnumerable<T> entities = await TryExecute(conn => conn.Table<T>().Where(x => filter(x)).ToListAsync());
		return entities;
	}

	public async Task Add(T entity)
	{
		await TryExecute(conn => conn.InsertAsync(entity));
	}

	public async Task AddAll(IEnumerable<T> entities)
	{
		await TryExecute(conn => conn.InsertAllAsync(entities));
	}

	public async Task Update(T entity)
	{
		await TryExecute(conn => conn.UpdateAsync(entity));
	}

	public async Task UpdateAll(IEnumerable<T> entities)
	{
		await TryExecute(conn => conn.UpdateAllAsync(entities));
	}

	public async Task Remove(T entity)
	{
		await TryExecute(conn => conn.DeleteAsync(entity));
	}

	public async Task RemoveAll()
	{
		await TryExecute(conn => conn.DeleteAllAsync<T>());
	}

	private TResult TryExecute<TResult>(Func<SQLiteAsyncConnection, TResult> action)
	{
		return _database.TryExecute(action);
	}
}
