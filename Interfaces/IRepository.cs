using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace HaberSitesi.Interfaces
{
    public interface IRepository<T> where T : class
    {
        List<T> GetAll(params Expression<Func<T, object>>[] includeProperties);
        T? GetById(int id);
        void Add(T entity); 
        void Update(T entity);
        void Delete(int id);
        void Save();
    }
    
}
