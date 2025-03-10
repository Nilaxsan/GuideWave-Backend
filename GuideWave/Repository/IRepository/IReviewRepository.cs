using GuideWave.Models;
using System.Diagnostics.Metrics;

namespace GuideWave.Repository.IRepository
{
    public interface IReviewRepository : IGenericRepository<Review>
    {
        Task Update(Review enitiy);

    }
}
