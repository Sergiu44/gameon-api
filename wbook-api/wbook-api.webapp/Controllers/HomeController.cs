using Microsoft.AspNetCore.Mvc;
using wbook_api.WebApp.Code.Utils;

namespace wbook_api.webapp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        [HttpGet("get")]
        [Authorize]
        public IActionResult Index()
        {
            var user = HttpContext.User.Claims.ToList()[0];
            return Ok("Buna iubita mea cu rasul ei colorat");
        }
    }
}
