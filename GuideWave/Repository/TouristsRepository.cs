using GuideWave.Data;
using GuideWave.Models;
using GuideWave.Repository.IRepository;
using System.Diagnostics.Metrics;

namespace GuideWave.Repository
{
    public class TouristsRepository : GenericRepository<Tourists>,ITouristsRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TouristsRepository(ApplicationDbContext dbContext): base(dbContext)
        {
            _dbContext = dbContext;
            
        }

        public async Task Update(Tourists enitiy)
        {
            _dbContext.Tourists.Update(enitiy);

            await _dbContext.SaveChangesAsync();
        }
    }
}
