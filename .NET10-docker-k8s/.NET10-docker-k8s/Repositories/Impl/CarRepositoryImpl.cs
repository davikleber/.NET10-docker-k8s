using Net10.docker.k8s.Data;
using Net10.docker.k8s.Model;

namespace Net10.docker.k8s.Repositories.Impl
{
    public class CarRepositoryImpl : RepositoryGeneric<Car>, Net10.docker.k8s.Repositories.ICarRepository
    {
        public CarRepositoryImpl(ApplicationDbContext context) : base(context)
        {
        }
    }
}
