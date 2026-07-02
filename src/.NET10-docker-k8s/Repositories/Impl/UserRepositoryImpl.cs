using Net10.docker.k8s.Data;
using Net10.docker.k8s.Model;
using System.Linq;

namespace Net10.docker.k8s.Repositories.Impl
{
    public class UserRepositoryImpl : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepositoryImpl(ApplicationDbContext context)
        {
            _context = context;
        }

        public User? FindByUsername(string username)
        {
            return _context.Users.FirstOrDefault(u => u.Username == username);
        }

        public User Create(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }
    }
}
