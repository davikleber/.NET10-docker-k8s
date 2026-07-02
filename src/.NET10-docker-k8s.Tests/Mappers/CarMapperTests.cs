using System.Linq;
using FluentAssertions;
using Net10.docker.k8s.DTO.Mappers;
using Net10.docker.k8s.Model;
using Xunit;

namespace Net10.docker.k8s.Tests.Mappers
{
    public class CarMapperTests
    {
        [Fact]
        public void ToDto_Should_Map_All_Properties()
        {
            var car = new Car
            {
                Id = 1,
                Make = "Toyota",
                Model = "Corolla",
                Year = 2020,
                Color = "Blue",
                Vin = "VIN123"
            };

            var dto = car.ToDto();

            dto.Should().NotBeNull();
            dto.Id.Should().Be(car.Id);
            dto.Make.Should().Be(car.Make);
            dto.Model.Should().Be(car.Model);
            dto.Year.Should().Be(car.Year);
            dto.Color.Should().Be(car.Color);
            dto.Vin.Should().Be(car.Vin);
        }

        [Fact]
        public void List_ToDto_Should_Map_All_Items()
        {
            var cars = Enumerable.Range(1, 4).Select(i => new Car
            {
                Id = i,
                Make = $"Make{i}",
                Model = $"Model{i}",
                Year = 2000 + i,
                Color = $"Color{i}",
                Vin = $"VIN{i}"
            }).ToList();

            var dtos = cars.Select(c => c.ToDto()).ToList();

            dtos.Should().HaveCount(cars.Count);
            dtos.Select(d => d.Id).Should().BeEquivalentTo(cars.Select(c => c.Id));
        }
    }
}
