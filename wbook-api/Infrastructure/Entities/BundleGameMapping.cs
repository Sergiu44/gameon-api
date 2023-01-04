using System;
using System.Collections.Generic;

namespace Infrastructure.Entities
{
    public partial class BundleGameMapping
    {
        public Guid IdBundle { get; set; }
        public Guid? IdGame { get; set; }
        public Guid? IdVariant { get; set; }

        public virtual Game? GameNavigation { get; set; }
        public virtual GameVariant? VariantNavigation { get; set; }
    }
}
