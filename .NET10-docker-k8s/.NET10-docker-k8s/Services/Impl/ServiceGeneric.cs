using System.Collections.Generic;
using Net10.docker.k8s.Model;
using Net10.docker.k8s.Repositories;
using Microsoft.Extensions.Logging;

namespace Net10.docker.k8s.Services.Impl
{
    public class ServiceGeneric<T> : Net10.docker.k8s.Services.IServiceGeneric<T> where T : class, IEntity
    {
        protected readonly Net10.docker.k8s.Repositories.IRepositoryGeneric<T> _repository;
        protected readonly ILogger _logger;

        public ServiceGeneric(Net10.docker.k8s.Repositories.IRepositoryGeneric<T> repository, ILogger logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public T Create(T entity)
        {
            _logger.LogInformation("Creating {Type}", typeof(T).Name);
            return _repository.Create(entity);
        }

        public void Delete(long id)
        {
            _logger.LogInformation("Deleting {Type} {Id}", typeof(T).Name, id);
            _repository.Delete(id);
        }

        public List<T> FindAll()
        {
            _logger.LogInformation("Retrieving all {Type}", typeof(T).Name);
            return _repository.FindAll();
        }

        public T? FindById(long id)
        {
            _logger.LogInformation("Retrieving {Type} {Id}", typeof(T).Name, id);
            return _repository.FindById(id);
        }

        public T? Update(T entity)
        {
            _logger.LogInformation("Updating {Type} {Id}", typeof(T).Name, entity.Id);
            return _repository.Update(entity);
        }
    }
}
