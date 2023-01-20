using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.User
{
    public class UserPutModel
    {
        public string? FirstName { get; set; } = null!;
        public string? LastName { get; set; } = null!;
        public string? Phone { get; set; } = null!;
        public string? Email { get; set; } = null!;
        public string? Bio { get; set; }
        public string? Type { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? PostalCode { get; set; }
        public string? County { get; set; }
        public string? City { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
