using GuideWave.Data;
using GuideWave.Models;
using GuideWave.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace GuideWave.Repository
{
    public class GuideRepository : GenericRepository<Guide>, IGuideRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public GuideRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guide> GetByEmail(string email)
        {
            return await _dbContext.Guides.FirstOrDefaultAsync(a => a.Email == email);
        }

        public async Task Update(Guide enitiy)
        {
            _dbContext.Guides.Update(enitiy);

            await _dbContext.SaveChangesAsync();
        }

    }
}
    