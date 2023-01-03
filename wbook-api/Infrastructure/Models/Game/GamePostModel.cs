using Infrastructure.Models.GameVariant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.Game
{
    public class GamePostModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<GameVariantPostModel> GameVariants { get; set; }
        public string? Image { get; set; }
        public string? HoverImage{ get; set; }
    }
}
