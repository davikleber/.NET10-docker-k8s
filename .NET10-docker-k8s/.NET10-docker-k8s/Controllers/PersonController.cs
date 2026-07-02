using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Net10.docker.k8s.Services;
using Net10.docker.k8s.Model;

namespace Net10.docker.k8s.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IPersonServices _personService;

        public PersonController(IPersonServices personService)
        {
            _personService = personService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_personService.FindAll());
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            var person = _personService.FindById(id);
            if (person == null) return NotFound();
            return Ok(person);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Person person)
        {
            var createdPerson = _personService.Create(person);
            if (createdPerson == null) return NotFound();
            return Ok(createdPerson);
        }

        [HttpPut]
        public IActionResult Put([FromBody] Person person)
        {
            var updatedPerson = _personService.Update(person);
            if (updatedPerson == null) return NotFound();
            return Ok(updatedPerson);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            _personService.Delete(id);
            return NoContent();
        }
    }
}
