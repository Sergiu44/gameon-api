using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common.DTOs
{
    public class CurrentUserDto
    {
        public CurrentUserDto()
        {
            Admin = false;
        }

        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Disabled { get; set; }
        public bool Authenticated { get; set; }
        public bool Admin { get; set; }
    }
}
