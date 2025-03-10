using GuideWave.Models;

namespace GuideWave.Repository.IRepository
{
    public interface ITouristsRepository :IGenericRepository<Tourists>
    {
        Task Update(Tourists enitiy);

    }
}
