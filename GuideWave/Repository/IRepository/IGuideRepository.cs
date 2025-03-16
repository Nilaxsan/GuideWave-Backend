using GuideWave.Models;

namespace GuideWave.Repository.IRepository
{
    public interface IGuideRepository : IGenericRepository<Guide>
    {
        Task<Guide> GetByEmail(string email);

        Task Update(Guide enitiy);
    }   
}
