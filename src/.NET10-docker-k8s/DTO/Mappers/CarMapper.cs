using Net10.docker.k8s.DTO;
using Net10.docker.k8s.Model;

namespace Net10.docker.k8s.DTO.Mappers
{
    public static class CarMapper
    {
        public static CarDto ToDto(this Car model)
        {
            if (model == null) return null!;
            return new CarDto
            {
                Id = model.Id,
                Make = model.Make,
                Model = model.Model,
                Year = model.Year,
                Color = model.Color,
                Vin = model.Vin
            };
        }

        public static Car ToModel(this CarDto dto)
        {
            if (dto == null) return null!;
            return new Car
            {
                Id = dto.Id,
                Make = dto.Make,
                Model = dto.Model,
                Year = dto.Year,
                Color = dto.Color,
                Vin = dto.Vin
            };
        }

        public static void UpdateModel(this Car model, CarDto dto)
        {
            if (model == null || dto == null) return;
            model.Make = dto.Make;
            model.Model = dto.Model;
            model.Year = dto.Year;
            model.Color = dto.Color;
            model.Vin = dto.Vin;
        }
    }
}
