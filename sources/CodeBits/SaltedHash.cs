using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CodeBits
{
    /// <summary>
    /// Encapsulates the functionality required to compute and verify salted hashes.
    /// </summary>
    public sealed partial class SaltedHash
    {
        /// <summary>
        /// Computes a salted hash from the given password.
        /// </summary>
        /// <param name="password">The password from which to compute the salted hash</param>
        /// <returns>A SaltedHash instance containing the hash and salt values</returns>
        public static SaltedHash Compute(string password)
        {
            var saltBytes = new byte[32];
            new RNGCryptoServiceProvider().GetNonZeroBytes(saltBytes);
            string salt = ConvertToBase64String(saltBytes);
            byte[] passwordAndSaltBytes = Concat(password, saltBytes);
            string hash = ComputeHash(passwordAndSaltBytes);
            return new SaltedHash(hash, salt);
        }

        /// <summary>
        /// Verifies that the a password matches the specified hash and salt values
        /// </summary>
        /// <param name="password">The password to check</param>
        /// <param name="hash">The hash value to check against</param>
        /// <param name="salt">The salt value to check against</param>
        /// <returns></returns>
        public static bool Verify(string password, string hash, string salt)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);
            byte[] passwordAndSaltBytes = Concat(password, saltBytes);
            string hashAttempt = ComputeHash(passwordAndSaltBytes);
            return hash == hashAttempt;
        }

        private static string ComputeHash(byte[] bytes)
        {
            return ConvertToBase64String(SHA256.Create().ComputeHash(bytes));
        }

        private static string ConvertToBase64String(byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }

        private static byte[] Concat(string password, byte[] saltBytes)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            return passwordBytes.Concat(saltBytes).ToArray();
        }
    }

    public sealed partial class SaltedHash
    {
        private readonly string _hash;
        private readonly string _salt;

        private SaltedHash(string hash, string salt)
        {
            _hash = hash;
            _salt = salt;
        }

        /// <summary>
        /// The computed hash value as a base-64 encoded string
        /// </summary>
        public string Hash
        {
            get { return _hash; }
        }

        /// <summary>
        /// The computed salt value as a base-64 encoded string
        /// </summary>
        public string Salt
        {
            get { return _salt; }
        }
    }
}