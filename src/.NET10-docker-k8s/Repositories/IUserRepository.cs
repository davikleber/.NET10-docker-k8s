using Net10.docker.k8s.Model;

namespace Net10.docker.k8s.Repositories
{
    public interface IUserRepository
    {
        User? FindByUsername(string username);
        User Create(User user);
        void Update(User user);
    }
}
