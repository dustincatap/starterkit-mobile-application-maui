using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using StarterKit.Maui.Core.Domain.Models;

namespace StarterKit.Maui.Core.Data.Local;

public abstract class BaseRepository<T> : IRepository<T> where T : class, IEntity
{
	private readonly DbContext _context;

	protected BaseRepository(DbContext context)
	{
		_context = context;
	}

	// DbSet should be a getter to get the latest entities from the database
	// instead of initializing it in the constructor.
	private DbSet<T> DbSet => _context.Set<T>();

	// Enumerate entities as another collection if we are going to use LINQ
	// See https://learn.microsoft.com/en-us/answers/questions/530674/resolve-exception-firstordefault-could-not-be-tran
	// and https://learn.microsoft.com/en-us/ef/core/what-is-new/ef-core-3.x/breaking-changes#linq-queries-are-no-longer-evaluated-on-the-client
	private IEnumerable<T> Entities => TryExecute(dbSet => dbSet.ToList());

	public T? Get(Predicate<T> filter)
	{
		return Entities.FirstOrDefault(x => filter(x));
	}

	public IEnumerable<T> GetAll()
	{
		return Entities;
	}

	public IEnumerable<T> GetAll(Predicate<T> filter)
	{
		return Entities.Where(x => filter(x));
	}

	public void Add(T entity)
	{
		TryExecute(dbSet => dbSet.Add(entity));
	}

	public void AddAll(IEnumerable<T> entities)
	{
		TryExecute(dbSet => dbSet.AddRange(entities));
	}

	public void Update(T entity)
	{
		TryExecute(dbSet => dbSet.Update(entity));
	}

	public void UpdateAll(IEnumerable<T> entities)
	{
		TryExecute(dbSet => dbSet.UpdateRange(entities));
	}

	public void Remove(T entity)
	{
		TryExecute(dbSet => dbSet.Remove(entity));
	}

	public void RemoveAll(IEnumerable<T> entities)
	{
		TryExecute(dbSet => dbSet.RemoveRange(entities));
	}

	public Task<int> SaveChanges()
	{
		return TryExecute(_ => _context.SaveChangesAsync());
	}

	private void TryExecute(Action<DbSet<T>> action)
	{
		TryExecute(dbSet =>
		{
			action.Invoke(dbSet);
			return true;
		});
	}

	private TResult TryExecute<TResult>(Func<DbSet<T>, TResult> action)
	{
		try
		{
			return action.Invoke(DbSet);
		}
		catch (SqliteException ex)
		{
			throw new InvalidOperationException("An error occurred while executing the db action", ex);
		}
	}
}
