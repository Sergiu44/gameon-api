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
        public async Task<IActionResult> GetVariants()
        {
            var variants = await _gameVariantService.GetVariants();
            return Ok(variants);
        }

        [Authorize]
        [HttpPost("post")]
        public IActionResult PostVariantToGame([FromForm] GameVariantPostModel model)
        {
            _gameVariantService.AddVariant(model);
            return Ok();
        }

        [Authorize]
        [HttpPut("put/{gameVariantId}")]
        public IActionResult PutVariantGame([FromForm] GameVariantEditModel model, [FromRoute] int gameVariantId)
        {
            _gameVariantService.EditVariant(model, gameVariantId);
            return Ok();
        }

        [Authorize]
        [HttpPost("delete/{gameVariantId}")]
        public IActionResult DeleteVariant([FromRoute] int gameVariantId)
        {
            _gameVariantService.DeleteVariant(gameVariantId);
            return Ok();
        }

        [HttpGet("image/{varId}")]
        public IActionResult GetImg([FromRoute] int varId)
        {
            var model = _gameVariantService.GetImg(varId);

            return File(model, "image/jpg");
        }

        [HttpGet("hoverImage/{varId}")]
        public IActionResult GetHoverImg([FromRoute] int varId)
        {
            var model = _gameVariantService.GetHoverImg(varId);

            return File(model, "image/jpg");
        }
    }
}
