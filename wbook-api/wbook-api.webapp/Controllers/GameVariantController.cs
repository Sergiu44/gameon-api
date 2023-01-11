using Infrastructure.Common.Base;
using Infrastructure.Models.Game;
using Infrastructure.Models.GameVariant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace wbook_api.webapp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GameVariantController : ControllerBase
    {
        private readonly GameVariantService _gameVariantService;
        public GameVariantController(GameVariantService gameVariantService)
        {
            _gameVariantService = gameVariantService;
        }

        [HttpGet("get")]
        [Authorize]
        public async Task<IActionResult> GetGameVariants([FromRoute] int gameId)
        {
            var variants = await _gameVariantService.GetGameVariants(gameId);
            return Ok(variants);
        }

        [HttpPost("post")]
        public IActionResult PostVariantToGame([FromForm] GameVariantPostModel model)
        {
            _gameVariantService.AddVariant(model);
            return Ok();
        }

        [HttpPost("delete")]
        public IActionResult DeleteVariant([FromBody] int gameId)
        {
            _gameVariantService.DeleteVariant(gameId);
            return Ok();
        }
    }
}
