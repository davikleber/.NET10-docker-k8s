using Net10.docker.k8s.Model;

namespace Net10.docker.k8s.Services
{
    public interface IUserService
    {
        User? GetByUsername(string username);
        User Create(User user);
        void Update(User user);
    }
}
