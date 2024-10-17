using GTERP.Models.Base;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Interfaces.Base
{
    public interface IBaseRepository<T> where T : BaseModel
    {
        IQueryable<T> GetAll();
        T Find(string ComId);
        T FindById(int? Id);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void RemoveRange(List<T> entity);
        void Remove(T entity);
        void AddRange(List<T> entity);
        void AddRange(ICollection<T> entity);
    }
}
