using Zwemclub.Data.Repository.Interfaces;

namespace Zwemclub.Data.Repository
{
    public class CustomUserRepository : GenericRepository<CustomUser>, ICustomUserRepository
    {
        public CustomUserRepository(ZwemclubContext zwemclubContext): base(zwemclubContext)
        {
            
        }

        public void Add(CustomUser model)
        {
            throw new NotImplementedException();
        }

        public void Delete(CustomUser model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<CustomUser>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<CustomUser> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(List<CustomUser> entities)
        {
            throw new NotImplementedException();
        }

        public void Update(CustomUser model)
        {
            throw new NotImplementedException();
        }
    }
}
