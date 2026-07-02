using System.Linq;
using FluentAssertions;
using Net10.docker.k8s.DTO.Mappers;
using Net10.docker.k8s.Model;
using Xunit;

namespace Net10.docker.k8s.Tests.Mappers
{
    public class PersonMapperTests
    {
        [Fact]
        public void ToDto_Should_Map_All_Properties()
        {
            var person = new Person
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Address = "123 Main St",
                Gender = "Male"
            };

            var dto = person.ToDto();

            dto.Should().NotBeNull();
            dto.Id.Should().Be(person.Id);
            dto.FirstName.Should().Be(person.FirstName);
            dto.LastName.Should().Be(person.LastName);
            dto.Address.Should().Be(person.Address);
            dto.Gender.Should().Be(person.Gender);
        }

        [Fact]
        public void List_ToDto_Should_Map_All_Items()
        {
            var persons = Enumerable.Range(1, 3).Select(i => new Person
            {
                Id = i,
                FirstName = $"First{i}",
                LastName = $"Last{i}",
                Address = $"Addr{i}",
                Gender = i % 2 == 0 ? "Female" : "Male"
            }).ToList();

            var dtos = persons.Select(p => p.ToDto()).ToList();

            dtos.Should().HaveCount(persons.Count);
            for (int i = 0; i < persons.Count; i++)
            {
                dtos[i].Id.Should().Be(persons[i].Id);
            }
        }
    }
}
