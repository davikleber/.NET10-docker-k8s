using Net10.docker.k8s.Model;
using Net10.docker.k8s.Repositories;

namespace Net10.docker.k8s.Services.Impl
{
    public class UserServiceImpl : IUserService
    {
        private readonly IUserRepository _repository;

        public UserServiceImpl(IUserRepository repository)
        {
            _repository = repository;
        }

        public User Create(User user)
        {
            return _repository.Create(user);
        }

        public User? GetByUsername(string username)
        {
            return _repository.FindByUsername(username);
        }

        public void Update(User user)
        {
            _repository.Update(user);
        }
    }
}
