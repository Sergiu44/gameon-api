using Microsoft.AspNetCore.Mvc;
using Services;

namespace wbook_api.webapp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WishlistController : ControllerBase
    {
        private readonly WishlistService _wishlistService;

        public WishlistController(WishlistService wishlistService)
        {
            _wishlistService= wishlistService;
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetWishlist()
        {
            var userId = Int32.Parse(HttpContext.User.Claims.ToList()[0].Value);
            var wishlistItems = await _wishlistService.GetVariantsForWishlist(userId);
            return Ok(wishlistItems);
        }

        [HttpPost("post/variant/{itemId}")]
        public IActionResult PostVariantInWishlist([FromRoute] int itemId)
        {
            var userId = Int32.Parse(HttpContext.User.Claims.ToList()[0].Value);
            _wishlistService.AddProductToWishlist(userId, itemId, true);
            return Ok();
        }

        [HttpPost("post/bundle/{itemId}")]
        public IActionResult PostBundleInWishlist([FromRoute] int itemId)
        {
            var userId = Int32.Parse(HttpContext.User.Claims.ToList()[0].Value);
            _wishlistService.AddProductToWishlist(userId, itemId, false);
            return Ok();
        }

        [HttpDelete("delete/variant/{id}")]
        public IActionResult DeleteItemFromWishlist([FromRoute] int id)
        {
            var userId = Int32.Parse(HttpContext.User.Claims.ToList()[0].Value);
            _wishlistService.DeleteProductFromWishlist(userId, id, true);
            return Ok();
        }

        [HttpDelete("delete/bundle/{id}")]
        public IActionResult DeleteBundleFromWishlist([FromRoute] int id)
        {
            var userId = Int32.Parse(HttpContext.User.Claims.ToList()[0].Value);
            _wishlistService.DeleteProductFromWishlist(userId, id, false);
            return Ok();
        }
    }
}
