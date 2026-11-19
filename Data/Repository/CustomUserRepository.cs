using Zwemclub.Data.Repository.Interfaces;

namespace Zwemclub.Data.Repository
{
    public class CustomUserRepository : ICustomUserRepository
    {
        private readonly ZwemclubContext _context;

        public CustomUserRepository(ZwemclubContext context)
        {
            _context = context;
        }

        public async Task<List<CustomUser>> GetAllAsync()
        {
            return await _context.Set<CustomUser>().ToListAsync();
        }

        public async Task<CustomUser?> GetByIdAsync(string id)
        {
            return await _context.Set<CustomUser>().FindAsync(id);
        }

        public void Add(CustomUser user)
        {
            _context.Set<CustomUser>().Add(user);
        }

        public void Update(CustomUser user)
        {
            _context.Set<CustomUser>().Update(user);
        }

        public void Delete(CustomUser user)
        {
            _context.Set<CustomUser>().Remove(user);
        }

        public async Task<bool> ExistsAsync(string id)
        {
            return await GetByIdAsync(id) != null;
        }

        public void RemoveRange(List<CustomUser> users)
        {
            _context.Set<CustomUser>().RemoveRange(users);
        }
    }
}
