using Infrastructure.Models.Game;
using Infrastructure.Models.GameVariant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.Bundle
{
    public class BundleItemModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int Price { get; set; }
        public int? Rrp { get; set; }
        public List<Tuple<GameVariantItemModel, GameModel>> GameVariants { get; set; }
    }
}
