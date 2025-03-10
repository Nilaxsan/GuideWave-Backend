using System.Linq.Expressions;

namespace GuideWave.Repository.IRepository
{
    
        public interface IGenericRepository<T> where T : class
        {

            Task<List<T>> GetAll();

            Task<T> Get(int id);

            Task Create(T enitiy);

            Task Delete(T enitiy);

            Task Save();

            bool IsRecordsExists(Expression<Func<T, bool>> condition);
        }
    
}

