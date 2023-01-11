using Infrastructure.Common.Base;
using Infrastructure.Models.Game;
using Microsoft.AspNetCore.Mvc;
using Services;
using wbook_api.WebApp.Code.Utils;

namespace wbook_api.webapp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly GameService _gameService;
        public GameController(GameService gameService)
        {
            _gameService = gameService;
        }

        [HttpGet("get")]
        [Authorize]
        public async Task<IActionResult> GetGames()
        {
            var games = await _gameService.GetGames();
            return Ok(games);
        }

        [HttpGet("getById")]
        public IActionResult GetGameById(int id)
        {
            var game = _gameService.GetGameById(id);
            return Ok(game);
        }

        [HttpPost("post")]
        [Authorize]
        public IActionResult PostGame([FromBody] GamePostModel model)
        {
            _gameService.AddGame(model);
            return Ok();
        }

        [HttpDelete("delete/{gameId}")]
        public IActionResult DeleteGame([FromRoute] int gameId)
        {
            _gameService.DeleteGame(gameId);
            return Ok();
        }
    }
}
