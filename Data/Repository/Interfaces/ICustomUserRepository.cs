using Zwemclub.Data.Repository.Interfaces;

namespace Zwemclub.Data.Repository
{
    public interface ICustomUserRepository
    {
        Task<List<CustomUser>> GetAllAsync();

        Task<CustomUser?> GetByIdAsync(string id);

        void Add(CustomUser user);

        void Update(CustomUser user);

        void Delete(CustomUser user);

        Task<bool> ExistsAsync(string id);

        void RemoveRange(List<CustomUser> users);
    }
}
