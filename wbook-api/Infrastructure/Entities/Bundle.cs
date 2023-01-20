using System;
using System.Collections.Generic;

namespace Infrastructure.Entities
{
    public partial class Bundle : IEntity
    {
        public Bundle()
        {
            BasketItems = new HashSet<BasketItem>();
            WishlistItems = new HashSet<WishlistItem>();
        }

        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public double Price { get; set; }
        public double? Rrp { get; set; }
        public byte[] Image { get; set; } = null!;
        public string? Category { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<BasketItem> BasketItems { get; set; }
        public virtual ICollection<WishlistItem> WishlistItems { get; set; }
    }
}
