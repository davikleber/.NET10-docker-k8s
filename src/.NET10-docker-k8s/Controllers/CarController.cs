using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Net10.docker.k8s.Model;
using Net10.docker.k8s.Services;

namespace Net10.docker.k8s.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CarController : ControllerBase
    {
        private readonly ICarServices _carService;

        public CarController(ICarServices carService)
        {
            _carService = carService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_carService.FindAll());
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            var car = _carService.FindById(id);
            if (car == null) return NotFound();
            return Ok(car);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Car car)
        {
            var created = _carService.Create(car);
            return Ok(created);
        }

        [HttpPut]
        public IActionResult Put([FromBody] Car car)
        {
            var updated = _carService.Update(car);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            _carService.Delete(id);
            return NoContent();
        }
    }
}
