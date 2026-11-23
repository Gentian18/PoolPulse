using Zwemclub.ViewModels;

namespace Zwemclub.Data.Repository.Interfaces
{
    public interface IWedstrijdRepository : IGenericRepository<Wedstrijd>
    {
        Task<List<WedstrijdOverzichtViewModel>> GetPubliekOverzichtAsync();
        Task<WedstrijdDetailsViewModel?> GetPubliekeDetailsAsync(int id);
    }
}
