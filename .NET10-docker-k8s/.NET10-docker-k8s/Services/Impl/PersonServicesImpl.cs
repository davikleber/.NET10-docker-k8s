using Net10.docker.k8s.Model;
using System.Collections.Generic;
using System;
using System.Linq;
using Net10.docker.k8s.Services;
using Net10.docker.k8s.Repositories;
using Microsoft.Extensions.Logging;

namespace Net10.docker.k8s.Services.Impl
{
    public class PersonServicesImpl : ServiceGeneric<Person>, IPersonServices
    {
        private readonly IPersonRepository _personRepository;

        public PersonServicesImpl(IPersonRepository repository, ILogger<PersonServicesImpl> logger)
            : base(repository, logger)
        {
            _personRepository = repository;

            // Seed some data if repository is empty
            if (!_personRepository.FindAll().Any())
            {
                for (int i = 1; i <= 5; i++)
                {
                    _personRepository.Create(new Person
                    {
                        FirstName = "John " + i,
                        LastName = "Doe " + i,
                        Address = $"123 Main Street {i}",
                        Gender = "Male"
                    });
                }
            }
        }
    }
}
