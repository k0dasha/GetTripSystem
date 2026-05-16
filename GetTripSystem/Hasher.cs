using BCrypt;
using System;
using System.Collections.Generic;
using System.Text;

namespace GetTripSystem
{
    public class Hasher
    {
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
        public static string HashPicture()
        {
            return null;
        }
    }
}
