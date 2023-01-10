using System;
using System.Collections.Generic;

namespace Infrastructure.Entities
{
    public partial class BundleGameMapping : IEntity
    {
        public int IdBundle { get; set; }
        public int? IdGame { get; set; }
        public int? IdVariant { get; set; }

        public virtual Game? IdGameNavigation { get; set; }
        public virtual GameVariant? IdVariantNavigation { get; set; }
    }
}
