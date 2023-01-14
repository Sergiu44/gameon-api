using Microsoft.AspNetCore.Http;

namespace Infrastructure.Models.Game
{
    public class GamePutModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public double? Rrp { get; set; }
        public IFormFile Image { get; set; }
        public IFormFile? HoverImage { get; set; }
    }
}
