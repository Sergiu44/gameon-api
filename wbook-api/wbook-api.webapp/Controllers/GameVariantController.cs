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

        [HttpPost("post")]
        public IActionResult PostVariantToGame([FromForm] GameVariantPostModel model)
        {
            _gameVariantService.AddVariant(model);
            return Ok();
        }

        [HttpPut("put/{gameVariantId}")]
        public IActionResult PutVariantGame([FromForm] GameVariantEditModel model, [FromRoute] int gameVariantId)
        {
            _gameVariantService.EditVariant(model, gameVariantId);
            return Ok();
        }

        [HttpPost("delete/{gameVariantId}")]
        public IActionResult DeleteVariant([FromRoute] int gameVariantId)
        {
            _gameVariantService.DeleteVariant(gameVariantId);
            return Ok();
        }

        [HttpGet("image")]
        public IActionResult GetVariantImg(int gameVariantId)
        {
            var model = _gameVariantService.GetImg(gameVariantId);

            var images = new List<FileContentResult>
            {
                File(model[0], "image/jpg"),
                File(model[1], "image/jpg")
            };

            return Ok(images);
        }
    }
}
