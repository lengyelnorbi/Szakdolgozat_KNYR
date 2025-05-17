using System;
using System.Security.Cryptography;
using System.Text;

namespace Szakdolgozat.Specials
{
    public static class PasswordHasher
    {
        // This salt should ideally be unique per user and stored in the database
        // For simplicity, we're using a fixed salt here
        private static readonly string _fixedSalt = "5z4kD0l60z4t5@lt"; // App-specific salt

        /// <summary>
        /// Hashes a password using SHA-256 with a fixed salt
        /// </summary>
        /// <param name="password">The password to hash</param>
        /// <returns>Hashed password as a hexadecimal string</returns>
        public static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password));

            // Combine password with salt
            string saltedPassword = password + _fixedSalt;

            using (SHA256 sha256 = SHA256.Create())
            {
                // Convert the salted password to bytes
                byte[] passwordBytes = Encoding.UTF8.GetBytes(saltedPassword);

                // Compute the hash
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);

                // Convert the hash bytes to a hexadecimal string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

        /// <summary>
        /// Verifies if a password matches a hash
        /// </summary>
        /// <param name="password">The password to check</param>
        /// <param name="hash">The hash to compare against</param>
        /// <returns>True if the password matches the hash</returns>
        public static bool VerifyPassword(string password, string hash)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hash))
                return false;

            string passwordHash = HashPassword(password);
            return passwordHash.Equals(hash);
        }
    }
}
