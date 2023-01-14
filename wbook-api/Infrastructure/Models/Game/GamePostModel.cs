using Infrastructure.Models.GameVariant;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.Game
{
    public class GamePostModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
        public IFormFile? HoverImage{ get; set; }
        public double Price { get; set; }
        public double? Rrp { get; set; }
        public string Type { get; set; }
    }
}
