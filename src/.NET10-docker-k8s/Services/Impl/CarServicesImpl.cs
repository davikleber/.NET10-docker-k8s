using Net10.docker.k8s.Model;
using Net10.docker.k8s.Repositories;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Net10.docker.k8s.Services.Impl
{
    public class CarServicesImpl : ServiceGeneric<Car>, ICarServices
    {
        public CarServicesImpl(ICarRepository repository, ILogger<CarServicesImpl> logger)
            : base(repository, logger)
        {
        }
    }
}
