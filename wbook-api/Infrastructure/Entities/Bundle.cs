﻿using System;
using System.Collections.Generic;

namespace Infrastructure.Entities
{
    public partial class Bundle
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Price { get; set; }
        public int? Rrp { get; set; }
        public string Image { get; set; } = null!;
        public string? Category { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
