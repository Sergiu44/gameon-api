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

        [HttpPost("post/{itemId}")]
        public IActionResult PostItemInBasket([FromRoute] int itemId)
        {
            var userId = Int32.Parse(HttpContext.User.Claims.ToList()[0].Value);
            _basketService.AddProductToBasket(userId, itemId);
            return Ok();
        }

        [HttpDelete("delete/{id}")]
        public IActionResult DeleteItemFromBasket([FromRoute] int id)
        {
            var userId = Int32.Parse(HttpContext.User.Claims.ToList()[0].Value);
            _basketService.DeleteProductFromBasket(userId, id);
            return Ok();
        }
    }
}
