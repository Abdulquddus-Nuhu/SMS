using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers.SPE
{
    [Route("api/[controller]")]
    [ApiController]
    public class VdaluesController : ControllerBase
    {
        // GET: api/<VdaluesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<VdaluesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<VdaluesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<VdaluesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<VdaluesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
