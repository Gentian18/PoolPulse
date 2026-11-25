using Zwemclub.Data.Repository.Interfaces;

namespace Zwemclub.Data.Repository
{
    public interface IGebruikerRepository
    {
        Task<List<Gebruiker>> GetAllAsync();

        Task<Gebruiker?> GetByIdAsync(string id);

        void Add(Gebruiker user);

        void Update(Gebruiker user);

        void Delete(Gebruiker user);

        Task<bool> ExistsAsync(string id);

        void RemoveRange(List<Gebruiker> users);
    }
}
