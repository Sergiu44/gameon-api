using Infrastructure.Models.Game;
using Infrastructure.Models.GameVariant;

namespace Infrastructure.Models.Bundle
{
    public class BundleGamesModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public double? Rrp { get; set; }
        public List<GameVariantBundleModel> GameVariants { get; set; }
    }
}
