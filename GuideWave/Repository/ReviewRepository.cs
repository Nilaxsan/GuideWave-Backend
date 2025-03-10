using GuideWave.Data;
using GuideWave.Models;
using GuideWave.Repository.IRepository;
using System.Diagnostics.Metrics;

namespace GuideWave.Repository
{
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository

    {
        private readonly ApplicationDbContext _dbContext;

        public ReviewRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task Update(Review enitiy)
        {
            _dbContext.Reviews.Update(enitiy);

            await _dbContext.SaveChangesAsync();
        }
    }
}
