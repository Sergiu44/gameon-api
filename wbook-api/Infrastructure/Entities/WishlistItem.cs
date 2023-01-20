using System;
using System.Collections.Generic;

namespace Infrastructure.Entities
{
    public partial class WishlistItem : IEntity
    {
        public int IdUser { get; set; }
        public int? IdVariant { get; set; }
        public int? IdBundle { get; set; }
        public int Id { get; set; }

        public virtual Bundle? IdBundleNavigation { get; set; }
        public virtual User IdUserNavigation { get; set; } = null!;
        public virtual GameVariant? IdVariantNavigation { get; set; }
    }
}
