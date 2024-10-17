using GTERP.Models.Self;
using System.Collections.Generic;
using System.Linq;

namespace GTERP.Interfaces.Self
{
    public interface ISelfRepository<T> where T : SelfModel
    {
        IQueryable<T> All();
        T Find(string ComId);
        T FindById(int? Id);
        int Insert(T model);

        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void RemoveRange(List<T> entity);


    }
}
