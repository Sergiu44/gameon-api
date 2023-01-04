using System;
using System.Collections.Generic;

namespace Infrastructure.Entities
{
    public partial class WishlistItem
    {
        public Guid IdUser { get; set; }
        public Guid? IdVariant { get; set; }
        public Guid? IdBundle { get; set; }

        public virtual Bundle? BundleNavigation { get; set; }
        public virtual User UserNavigation { get; set; } = null!;
        public virtual GameVariant? VariantNavigation { get; set; }
    }
}
