using System.Collections.Generic;
using Net10.docker.k8s.Model;

namespace Net10.docker.k8s.Repositories
{
    public interface IRepositoryGeneric<T> where T : IEntity
    {
        T Create(T entity);

        T? FindById(long id);

        List<T> FindAll();

        T? Update(T entity);

        void Delete(long id);
    }
}
