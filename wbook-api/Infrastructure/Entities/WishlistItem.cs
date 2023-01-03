using System;
using System.Collections.Generic;

namespace Infrastructure.Entities
{
    public partial class WishlistItem
    {
        public int IdUser { get; set; }
        public int IdVariant { get; set; }

        public virtual User IdUser1 { get; set; } = null!;
        public virtual GameVariant IdUserNavigation { get; set; } = null!;
    }
}
