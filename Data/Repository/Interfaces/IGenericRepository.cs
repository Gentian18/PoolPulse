namespace Zwemclub.Data.Repository.Interfaces
{
    public interface IGenericRepository<T> where T : class, IModel
    {
        Task<List<T>> GetAllAsync();

        Task<T?> GetByIdAsync(int id);

        void Add(T model);

        void Update(T model);

        void Delete(T model);

        Task<bool> ExistsAsync(int id);

        void RemoveRange(List<T> entities);
    }
}
