using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace TemplatesSample.Controllers
{
    [Produces("application/json")]
    [Route("api/Values")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET: api/Values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            var result = new [] { "value1", "value2" };
            return Ok(result);
        }

        // GET: api/Values/5
        [HttpGet("{id}", Name = nameof(Get))]
        public ActionResult<string> Get(int id)
        {
            var result = "value";
            return Ok(result);
        }
        
        // POST: api/Values
        [HttpPost]
        public ActionResult<string> Post([FromBody] string value)
        {
            return CreatedAtAction(nameof(Get), new { id = 5 }, value);
        }
        
        // PUT: api/Values/5
        [HttpPut("{id}")]
        public ActionResult<string> Put(int id, [FromBody] string value)
        {
            return Ok(value);
        }
        
        // DELETE: api/Values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return NoContent();
        }
    }
}
