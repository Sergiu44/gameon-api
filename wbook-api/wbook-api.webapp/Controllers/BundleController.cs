using Infrastructure.Models.Bundle;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace wbook_api.webapp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BundleController : ControllerBase
    {
        private readonly BundleService _bundleService;
        public BundleController(BundleService bundleService)
        {
            _bundleService = bundleService;
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetBundles()
        {
            var bundles = _bundleService.GetBundles();
            return Ok(bundles);
        }

        [HttpGet("get/{bundleId}")]
        public IActionResult GetBundleById(int bundleId)
        {
            var bundle = _bundleService.GetBundleById(bundleId);
            return Ok(bundle);
        }

        [HttpPost("post")]
        public async Task<IActionResult> PostBundle([FromForm] BundlePostModel bundle)
        {
            await _bundleService.AddBundle(bundle);
            return Ok();
        }

        [HttpPost("{bundleId}/post/{gameId}")]
        public IActionResult PostToBundle([FromRoute] int bundleId, [FromRoute] int gameId)
        {
            _bundleService.AddToBundle(bundleId, gameId);
            return Ok();
        }

        [HttpPost("{bundleId}/delete/{gameId}")]
        public IActionResult DeleteFromBundle([FromRoute] int bundleId, [FromRoute] int gameId)
        {
            _bundleService.RemoveFromBundle(bundleId, gameId);
            return Ok();
        }

        [HttpDelete("delete/{bundleId}")]
        public IActionResult DeleteBundle([FromRoute] int bundleId)
        {
            _bundleService.DeleteBundle(bundleId);
            return Ok();
        }

        [HttpGet("image")]
        public IActionResult GetImg(int bundleId)
        {
            var model = _bundleService.GetImg(bundleId);

            return File(model, "image/jpg");
        }
    }
}
