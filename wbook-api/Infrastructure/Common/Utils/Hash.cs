using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common.Utils
{
    public static class Hash
    {
        public static byte[] HashPassword(this string password, Guid UserSalt)
        {
            var salt = UserSalt.ToString();
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] passwordHashed = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + salt));

                return passwordHashed;
            }
        }
    }
}
