using Microsoft.AspNetCore.Mvc;

namespace UsingSaga.Worker.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InfraController : ControllerBase
    {

        public InfraController()
        {

        }

        [HttpGet()]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}
