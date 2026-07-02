using Net10.docker.k8s.Model;
using System.Collections.Generic;

namespace Net10.docker.k8s.Services
{
    public interface ICarServices
    {
        Car Create(Car car);

        Car? FindById(long id);

        List<Car> FindAll();

        Car? Update(Car car);

        void Delete(long id);
    }
}
