using Net10.docker.k8s.Model;
using System.Collections.Generic;

namespace Net10.docker.k8s.Services
{
    public interface IPersonServices
    {
        Person Create(Person person);

        Person? FindById(long id);

        List<Person> FindAll();

        Person? Update(Person person);

        void Delete(long id);
    }
}
