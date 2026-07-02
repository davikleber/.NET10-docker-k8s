using Net10.docker.k8s.Data;
using Net10.docker.k8s.Model;

namespace Net10.docker.k8s.Repositories.Impl
{
    public class PersonRepositoryImpl : RepositoryGeneric<Person>, Net10.docker.k8s.Repositories.IPersonRepository
    {
        public PersonRepositoryImpl(ApplicationDbContext context) : base(context)
        {
        }
    }
}
