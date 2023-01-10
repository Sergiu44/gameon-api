using Infrastructure.Common.Base;
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

        public BasketController(ControllerDependencies dependencies, BasketService basketService)
        {
            _basketService = basketService;
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetBasket([FromBody] int userId)
        {
            var basketItems = await _basketService.GetVariantsForBasket(userId);
            return Ok(basketItems);
        }

        [HttpPost("post")]
        [Authorize]
        public IActionResult PostItemInBasket([FromBody] int itemId)
        {
            var userId = Int32.Parse(HttpContext.User.Claims.ToList()[0].Value);
            _basketService.AddProductToBasket(userId, itemId);
            return Ok();
        }

        [HttpDelete("delete")]
        public IActionResult DeleteItemFromBasket([FromBody] int id)
        {
            _basketService.DeleteProductFromBasket(id);
            return Ok();
        }
    }
}
