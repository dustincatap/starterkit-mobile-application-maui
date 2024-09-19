using StarterKit.Maui.Core.Domain.Models;

namespace StarterKit.Maui.Core.Data.Local;

public interface IRepository<T> where T : class, IEntity, new()
{
	Task<T?> Get(Predicate<T> filter);

	Task<IEnumerable<T>> GetAll();

	Task<IEnumerable<T>> GetAll(Predicate<T> filter);

	Task Add(T entity);

	Task AddAll(IEnumerable<T> entities);

	Task Update(T entity);

	Task UpdateAll(IEnumerable<T> entities);

	Task Remove(T entity);

	Task RemoveAll();
}
