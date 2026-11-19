using Zwemclub.Data.Repository.Interfaces;

namespace Zwemclub.Data.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class, IModel
    {
        protected readonly ZwemclubContext _context;

        public GenericRepository(ZwemclubContext context)
        {
            _context = context;
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public void Add(T model)
        {
            _context.Set<T>().Add(model);
        }

        public void Update(T model)
        {
            _context.Set<T>().Update(model);
        }

        public void Delete(T model)
        {
            _context.Set<T>().Remove(model);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await GetByIdAsync(id) != null;
        }

        public void RemoveRange(List<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }
    }
}
