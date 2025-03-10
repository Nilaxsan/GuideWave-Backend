using GuideWave.Data;
using GuideWave.Models;
using GuideWave.Repository.IRepository;

namespace GuideWave.Repository
{
    public class GuideRepository : GenericRepository<Guide>, IGuideRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public GuideRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task Update(Guide enitiy)
        {
            _dbContext.Guides.Update(enitiy);

            await _dbContext.SaveChangesAsync();
        }

    }
}
    