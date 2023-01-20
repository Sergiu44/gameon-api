using Infrastructure.Models.GameVariant;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Models.Bundle
{
    public class BundlePostModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public int Price { get; set; }
        public int Rrp { get; set; }
        public IFormFile Image { get; set; }
        public IFormFile? HoverImage { get; set; }
        public List<int> GameVariantsId { get; set; }
    }
}
