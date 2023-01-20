using System;
using System.Collections.Generic;

namespace Infrastructure.Entities
{
    public partial class Game : IEntity
    {
        public Game()
        {
            GameVariants = new HashSet<GameVariant>();
        }

        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public double Price { get; set; }
        public double? Rrp { get; set; }
        public byte[] Image { get; set; } = null!;
        public byte[]? HoverImage { get; set; }
        public string? Category { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<GameVariant> GameVariants { get; set; }
    }
}
