using GuideWave.Models;

namespace GuideWave.Repository.IRepository
{
    public interface IGuideRepository : IGenericRepository<Guide>
    {
        Task Update(Guide enitiy);

    }   
}
