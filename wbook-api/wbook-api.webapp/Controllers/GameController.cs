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
        public async Task<IActionResult> GetGames()
        {
            var games = await _gameService.GetGames();
            return Ok(games);
        }

        [HttpGet("get/{gameId}")]
        public IActionResult GetGameById(int gameId)
        {
            var game = _gameService.GetGameById(gameId);
            return Ok(game);
        }

        [HttpPost("post")]
        public async Task<IActionResult> PostGame([FromForm] GamePostModel model)
        {
            await _gameService.AddGame(model);
            return Ok();
        }

        [HttpPut("put/{gameId}")]
        public IActionResult PutGame([FromForm] GamePutModel model, [FromRoute] int gameId)
        {
            _gameService.EditGame(model, gameId);
            return Ok();
        }

        [HttpDelete("delete/{gameId}")]
        public IActionResult DeleteGame([FromRoute] int gameId)
        {
            _gameService.DeleteGame(gameId);
            return Ok();
        }

        [HttpGet("image/{gameId}")]
        public IActionResult GetImg([FromRoute] int gameId)
        {
            var model = _gameService.GetImg(gameId);

            return File(model, "image/jpg");
        }
    }
}
