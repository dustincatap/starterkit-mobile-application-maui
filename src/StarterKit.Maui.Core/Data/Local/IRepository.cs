namespace StarterKit.Maui.Core.Data.Local;

public interface IRepository<T> where T : class
{
    T? Get(Predicate<T> predicate);

    IEnumerable<T> GetAll();

    IEnumerable<T> GetAll(Predicate<T> predicate);

    void Add(T entity);

    void AddAll(IEnumerable<T> entities);

    void Update(T entity);

    void UpdateAll(IEnumerable<T> entities);

    void Remove(T entity);

    void RemoveAll(IEnumerable<T> entities);
    
    int SaveChanges();
}