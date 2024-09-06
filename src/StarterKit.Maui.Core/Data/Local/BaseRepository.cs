using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace StarterKit.Maui.Core.Data.Local;

public abstract class BaseRepository<T> : IRepository<T> where T : class
{
    private const int SqliteGenericErrorCode = 1;

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
    private IEnumerable<T> Entities => CheckForOutdatedDatabase(dbSet => dbSet.ToList());

    public T? Get(Predicate<T> predicate)
    {
        return Entities.FirstOrDefault(x => predicate(x));
    }

    public IEnumerable<T> GetAll()
    {
        return Entities;
    }

    public IEnumerable<T> GetAll(Predicate<T> predicate)
    {
        return Entities.Where(x => predicate(x));
    }

    public void Add(T entity)
    {
        CheckForOutdatedDatabase(dbSet => dbSet.Add(entity));
    }

    public void AddAll(IEnumerable<T> entities)
    {
        CheckForOutdatedDatabase(dbSet => dbSet.AddRange(entities));
    }

    public void Update(T entity)
    {
        CheckForOutdatedDatabase(dbSet => dbSet.Update(entity));
    }

    public void UpdateAll(IEnumerable<T> entities)
    {
        CheckForOutdatedDatabase(dbSet => dbSet.UpdateRange(entities));
    }

    public void Remove(T entity)
    {
        CheckForOutdatedDatabase(dbSet => dbSet.Remove(entity));
    }

    public void RemoveAll(IEnumerable<T> entities)
    {
        CheckForOutdatedDatabase(dbSet => dbSet.RemoveRange(entities));
    }

    public int SaveChanges()
    {
        return CheckForOutdatedDatabase(_ => _context.SaveChanges());
    }

    private void CheckForOutdatedDatabase(Action<DbSet<T>> action)
    {
        CheckForOutdatedDatabase(dbSet =>
        {
            action.Invoke(dbSet);
            return true;
        });
    }

    private TResult CheckForOutdatedDatabase<TResult>(Func<DbSet<T>, TResult> action)
    {
        try
        {
            return action.Invoke(DbSet);
        }
        catch (SqliteException ex) when (ex.SqliteErrorCode == SqliteGenericErrorCode)
        {
            throw new InvalidOperationException("The database is outdated.", ex);
        }
    }
}