using HaberSitesi.Data;
using HaberSitesi.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


namespace HaberSitesi.Repositories
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {

        private readonly HaberContext _context;
        private readonly DbSet<T> _table;

        public GenericRepository(HaberContext context)
        {
            _context = context;
            _table = _context.Set<T>();
        }

        public List<T> GetAll(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _table;

            if(includeProperties != null)
            {
                foreach(var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }
            return query.ToList();
        }



        public T? GetById(int id)
        {
            return _table.Find(id);
        }

        public void Add(T entity)
        {
            _table.Add(entity);

            Save();
        }

        public void Update(T entity)
        {
            _table.Update(entity);

            Save();
        }

        public void Delete(int id)
        {
            var existing = _table.Find(id);

            if (existing != null)
            {
                _table.Remove(existing);
                Save();
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}



