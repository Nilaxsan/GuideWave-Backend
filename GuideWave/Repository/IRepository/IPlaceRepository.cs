using GuideWave.Models;

namespace GuideWave.Repository.IRepository
{
    public interface IPlaceRepository :IGenericRepository<Place>
    {
        Task Update (Place place);
    }
}
