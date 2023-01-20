using System;
using System.Collections.Generic;

namespace Infrastructure.Entities
{
    public partial class User : IEntity
    {
        public User()
        {
            BasketItems = new HashSet<BasketItem>();
            WishlistItems = new HashSet<WishlistItem>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Email { get; set; } = null!;
        public byte[] Password { get; set; } = null!;
        public Guid Salt { get; set; }
        public string? Bio { get; set; }
        public string? Type { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? PostalCode { get; set; }
        public string? County { get; set; }
        public string? City { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<BasketItem> BasketItems { get; set; }
        public virtual ICollection<WishlistItem> WishlistItems { get; set; }
    }
}
