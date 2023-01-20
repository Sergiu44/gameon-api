using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.GameVariant
{
    public class GameVariantEditModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public double? Price { get; set; }
        public double? Rrp { get; set; }
        public IFormFile? Image { get; set; }
        public IFormFile? HoverImage { get; set; }
    }
}
