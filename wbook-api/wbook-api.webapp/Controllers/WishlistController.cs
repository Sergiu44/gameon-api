using Infrastructure.Common.Base;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace wbook_api.webapp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WishlistController : ControllerBase
    {
        private readonly WishlistService _wishlistService;

        public WishlistController(ControllerDependencies dependencies, WishlistService wishlistService)
        {
            _wishlistService= wishlistService;
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetWishlist([FromBody] int userId)
        {
            var wishlistItems = await _wishlistService.GetVariantsForWishlist(userId);
            return Ok(wishlistItems);
        }

        [HttpPost("post")]
        public IActionResult PostItemInWishlist([FromBody] int id)
        {
            _wishlistService.AddProductToWishlist(id);
            return Ok();
        }

        [HttpDelete("delete")]
        public IActionResult DeleteItemFromWishlist([FromBody] int id)
        {
            _wishlistService.DeleteProductFromWishlist(id);
            return Ok();
        }
    }
}
