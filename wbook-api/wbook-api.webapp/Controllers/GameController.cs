using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace wbook_api.webapp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class GameController : ControllerBase
    {
        private readonly GameService _gameService;
        public GameController(GameService gameService)
        {
            _gameService = gameService;
        }
    }
}
