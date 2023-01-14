using System;
using System.Collections.Generic;

namespace Infrastructure.Entities
{
    public partial class Category : IEntity
    {
        public int Id { get; set; }
        public string CategoryName { get; set; } = null!;
    }
}
