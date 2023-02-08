using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace wbook_api.webapp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly BasketService _basketService;

        public BasketController(BasketService basketService)
        {
            _basketService = basketService;
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetBasket()
        {
            var userId = Int32.Parse(HttpContext.User.Claims.ToList()[0].Value);
            var basketItems = await _basketService.GetVariantsForBasket(userId);
            return Ok(basketItems);
        }

        [HttpPost("post/variant/{itemId}")]
        public IActionResult PostVariantInBasket([FromRoute] int itemId)
        {
            var userId = Int32.Parse(HttpContext.User.Claims.ToList()[0].Value);
            _basketService.AddProductToBasket(userId, itemId, true);
            return Ok();
        }

        [HttpPost("post/bundle/{itemId}")]
        public IActionResult PostBundleInBasket([FromRoute] int itemId)
        {
            var userId = Int32.Parse(HttpContext.User.Claims.ToList()[0].Value);
            _basketService.AddProductToBasket(userId, itemId, false);
            return Ok();
        }

        [HttpDelete("delete/variant/{id}")]
        public IActionResult DeleteVariantFromBasket([FromRoute] int id)
        {
            var userId = Int32.Parse(HttpContext.User.Claims.ToList()[0].Value);
            _basketService.DeleteProductFromBasket(userId, id, true);
            return Ok();
        }

        [HttpDelete("delete/bundle/{id}")]
        public IActionResult DeleteBundleFromBasket([FromRoute] int id)
        {
            var userId = Int32.Parse(HttpContext.User.Claims.ToList()[0].Value);
            _basketService.DeleteProductFromBasket(userId, id, false);
            return Ok();
        }
    }
}
