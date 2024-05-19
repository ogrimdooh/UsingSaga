using Microsoft.AspNetCore.Mvc;

namespace UsingSaga.WebApi.Controllers
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
