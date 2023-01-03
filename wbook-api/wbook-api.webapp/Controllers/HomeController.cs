using Microsoft.AspNetCore.Mvc;
using Services;

namespace wbook_api.webapp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly UserService _userService;

        public HomeController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet("GET")]
        public IActionResult Index()
        {
            return Ok(_userService.Test());
        }
    }
}
