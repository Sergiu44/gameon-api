using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.User
{
    public class UserPasswordModel
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
