using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Net10.docker.k8s.Data;
using Net10.docker.k8s.Model;

namespace Net10.docker.k8s.Repositories.Impl
{
    public class RepositoryGeneric<T> : Net10.docker.k8s.Repositories.IRepositoryGeneric<T> where T : class, IEntity
    {
        protected readonly ApplicationDbContext _context;

        public RepositoryGeneric(ApplicationDbContext context)
        {
            _context = context;
        }

        public T Create(T entity)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public void Delete(long id)
        {
            var existing = _context.Set<T>().Find((int)id);
            if (existing == null) return;
            _context.Set<T>().Remove(existing);
            _context.SaveChanges();
        }

        public List<T> FindAll()
        {
            return _context.Set<T>().ToList();
        }

        public T? FindById(long id)
        {
            return _context.Set<T>().Find((int)id);
        }

        public T? Update(T entity)
        {
            var existing = _context.Set<T>().Find(entity.Id);
            if (existing == null) return null;
            _context.Entry(existing).CurrentValues.SetValues(entity);
            _context.SaveChanges();
            return existing;
        }
    }
}
