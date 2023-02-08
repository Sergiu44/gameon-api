using System;
using System.Collections.Generic;

namespace Infrastructure.Entities
{
    public partial class GameVariant : IEntity
    {
        public GameVariant()
        {
            BasketItems = new HashSet<BasketItem>();
            WishlistItems = new HashSet<WishlistItem>();
        }

        public int Id { get; set; }
        public int GameId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public double Price { get; set; }
        public double? Rrp { get; set; }
        public byte[] Image { get; set; } = null!;
        public byte[]? HoverImage { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Game Game { get; set; } = null!;
        public virtual ICollection<BasketItem> BasketItems { get; set; }
        public virtual ICollection<WishlistItem> WishlistItems { get; set; }
    }
}
