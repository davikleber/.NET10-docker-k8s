using System;

namespace Net10.docker.k8s.Model
{
    public class Car : IEntity
    {
        public int Id { get; set; }
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public string Color { get; set; } = string.Empty;
        public string Vin { get; set; } = string.Empty;
    }
}
