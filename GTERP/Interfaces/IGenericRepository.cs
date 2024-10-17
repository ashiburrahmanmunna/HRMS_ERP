using System.Collections.Generic;
using System.Threading.Tasks;

namespace GTERP.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(object Id);
        Task InsertAsync(T obj);
        Task UpdateAsync(T obj);
        Task DeleteAsync(object Id);
        Task SaveChangesAsync();
    }
}
