namespace WebApp_identity.IRepositories;

public interface IRepository<T> where T : class
{
    Task<T> CreatAsync(T entity, CancellationToken cn = default);
}