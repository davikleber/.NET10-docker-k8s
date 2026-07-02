using Net10.docker.k8s.DTO;
using Net10.docker.k8s.Model;

namespace Net10.docker.k8s.DTO.Mappers
{
    public static class PersonMapper
    {
        public static PersonDto ToDto(this Person model)
        {
            if (model == null) return null!;
            return new PersonDto
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Address = model.Address,
                Gender = model.Gender
            };
        }

        public static Person ToModel(this PersonDto dto)
        {
            if (dto == null) return null!;
            return new Person
            {
                Id = dto.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Address = dto.Address,
                Gender = dto.Gender
            };
        }

        public static void UpdateModel(this Person model, PersonDto dto)
        {
            if (model == null || dto == null) return;
            model.FirstName = dto.FirstName;
            model.LastName = dto.LastName;
            model.Address = dto.Address;
            model.Gender = dto.Gender;
        }
    }
}
