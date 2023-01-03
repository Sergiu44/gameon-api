using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.GameVariant
{
    public class GameVariantItemModel
    {
        public Guid Id { get; set; }
        public Guid GameId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int Rrp { get; set; }
        public string Image { get; set; }
        public string? HoverImage { get; set; }
    }
}
