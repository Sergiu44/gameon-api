using System;
using System.Collections.Generic;

namespace Infrastructure.Entities
{
    public partial class Game
    {
        public Game()
        {
            GameVariants = new HashSet<GameVariant>();
        }

        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Price { get; set; }
        public int? Rrp { get; set; }
        public string Image { get; set; } = null!;
        public string? HoverImage { get; set; }
        public string? Category { get; set; }
        public Guid CheapestVariantId { get; set; }
        public string Url { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public virtual GameVariant CheapestVariant { get; set; } = null!;
        public virtual ICollection<GameVariant> GameVariants { get; set; }
    }
}
