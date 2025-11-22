using WebApp_identity.Data;
using WebApp_identity.IRepositories;

namespace WebApp_identity.Repository;

public class Repository<T> : IRepository<T> where T : class 
{
    private readonly ApplicationDbContext _context;
    public Repository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<T> CreatAsync(T entity, CancellationToken cn = default)
    {
        await _context.Set<T>().AddAsync(entity,cn);
        await _context.SaveChangesAsync(cn);
        return entity;
        
    }
    
}