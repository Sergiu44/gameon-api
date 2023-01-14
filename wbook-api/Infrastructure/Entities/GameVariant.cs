﻿using System;
using System.Collections.Generic;

namespace Infrastructure.Entities
{
    public partial class GameVariant : IEntity
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public double Price { get; set; }
        public double? Rrp { get; set; }
        public string Image { get; set; } = null!;
        public string? HoverImage { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Game Game { get; set; } = null!;
    }
}
