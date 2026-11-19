using Zwemclub.Data.Repository.Interfaces;

namespace Zwemclub.Data.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class, IModel
    {
        protected ZwemclubContext _context;

        public GenericRepository(ZwemclubContext zwemclubContext)
        {
            _context = zwemclubContext;
        }

        public void Add(T model)
        {
            throw new NotImplementedException();
        }

        public void Delete(T model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<T>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<T> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(List<T> entities)
        {
            throw new NotImplementedException();
        }

        public void Update(T model)
        {
            throw new NotImplementedException();
        }
    }
}
