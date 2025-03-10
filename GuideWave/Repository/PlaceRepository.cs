using GuideWave.Data;
using GuideWave.Models;
using GuideWave.Repository.IRepository;

namespace GuideWave.Repository
{
    public class PlaceRepository : GenericRepository<Place>,IPlaceRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public PlaceRepository(ApplicationDbContext dbContext):base(dbContext)
        {
            _dbContext = dbContext;
            
        }

        public async Task Update(Place enitiy)
        {
            _dbContext.Places.Update(enitiy);

            await _dbContext.SaveChangesAsync();
        }
    }
}
