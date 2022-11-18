using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace contemp_programming_final_project.Controllers
{
    [ApiController]
    [Route("api/shows/[controller]")]
    public class showsController : ControllerBase
    {
        // GET: api/<showsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<showsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<showsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<showsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<showsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
