using System;
using System.Collections.Generic;

namespace Infrastructure.Entities
{
    public partial class Bundle : IEntity
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public double Price { get; set; }
        public double? Rrp { get; set; }
        public string Image { get; set; } = null!;
        public string? Category { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
