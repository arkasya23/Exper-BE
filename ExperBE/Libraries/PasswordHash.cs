using System;
using System.Security.Cryptography;

namespace ExperBE.Libraries
{
    public static class PasswordHash
    {
        private const int SaltSize = 16; // 128 bit
        private const int HashSize = 32;
        private const int Iterations = 10000;

        public static (string hash, string salt) Encrypt(string password)
        {
            byte[] salt = GenerateSalt();
            string hash = Encrypt(password, salt);
            return (hash, Convert.ToBase64String(salt));
        }

        public static bool Verify(string password, string hashedPassword, string salt)
        {
            if (string.IsNullOrEmpty(password)
                || string.IsNullOrEmpty(hashedPassword)
                || string.IsNullOrEmpty(salt))
            {
                return false;
            }

            try
            {
                byte[] saltBytes = Convert.FromBase64String(salt);
                var hash = Encrypt(password, saltBytes);
                return hash == hashedPassword;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private static string Encrypt(string password, byte[] salt)
        {
            byte[] hash;
            using (Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations))
            {
                hash = pbkdf2.GetBytes(HashSize);
            }
            return Convert.ToBase64String(hash);
        }

        private static byte[] GenerateSalt()
        {
            byte[] salt = new byte[SaltSize];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(salt);
            return salt;
        }
    }
}
