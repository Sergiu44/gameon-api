using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.GameVariant
{
    public class GameVariantPostModel
    {
        public Guid Id { get; set; }
        public Guid GameId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
