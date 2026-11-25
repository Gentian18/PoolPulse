using Zwemclub.Data.Repository.Interfaces;
using Zwemclub.Data.Repository;

namespace Zwemclub.Data.Repository
{
    public class GebruikerRepository : IGebruikerRepository
    {
        private readonly ZwemclubContext _context;

        public GebruikerRepository(ZwemclubContext context)
        {
            _context = context;
        }

        public async Task<List<Gebruiker>> GetAllAsync()
        {
            return await _context.Set<Gebruiker>().ToListAsync();
        }

        public async Task<Gebruiker?> GetByIdAsync(string id)
        {
            return await _context.Set<Gebruiker>().FindAsync(id);
        }

        public void Add(Gebruiker user)
        {
            _context.Set<Gebruiker>().Add(user);
        }

        public void Update(Gebruiker user)
        {
            _context.Set<Gebruiker>().Update(user);
        }

        public void Delete(Gebruiker user)
        {
            _context.Set<Gebruiker>().Remove(user);
        }

        public async Task<bool> ExistsAsync(string id)
        {
            return await GetByIdAsync(id) != null;
        }

        public void RemoveRange(List<Gebruiker> users)
        {
            _context.Set<Gebruiker>().RemoveRange(users);
        }

    }
}
