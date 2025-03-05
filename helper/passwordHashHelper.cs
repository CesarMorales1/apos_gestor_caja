using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt;
using BCrypt.Net;

namespace apos_gestor_caja.Helpers
{
    class PasswordHashHelper
    {
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, 15);
        }

        public static bool VerifyPassword(string passwordToVerify,string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(passwordToVerify, hashedPassword);
        }
    }
}
