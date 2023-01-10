using Microsoft.AspNetCore.Mvc;

namespace wbook_api.webapp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BundleController : Controller
    {
        [HttpGet("get")]
        public IActionResult Index()
        {
            return Ok();
        }
    }
}
