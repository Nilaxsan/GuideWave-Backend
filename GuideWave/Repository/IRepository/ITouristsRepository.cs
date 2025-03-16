using GuideWave.Models;

namespace GuideWave.Repository.IRepository
{
    public interface ITouristsRepository :IGenericRepository<Tourists>
    {
        Task<Tourists> GetByEmail(string email);

        Task Update(Tourists enitiy);
    }
}
