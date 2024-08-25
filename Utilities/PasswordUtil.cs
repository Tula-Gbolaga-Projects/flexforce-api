using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;

namespace agency_portal_api.Utilities
{
    public static class PasswordUtil
    {
        public static byte[] CreateHash(string password, byte[] salt)
        {
            var _password = Encoding.ASCII.GetBytes(password);
            using var argon2 = new Argon2id(_password);
            argon2.Salt = salt;
            argon2.DegreeOfParallelism = 8;
            argon2.Iterations = 4;
            argon2.MemorySize = 1024 * 128;

            return argon2.GetBytes(32);
        }

        public static bool VerifyHash(string password, byte[] salt, byte[] hash) =>
            CreateHash(password, salt).SequenceEqual(hash);

        public static byte[] GenerateSalt()
        {
            var buffer = new byte[32];
            using var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(buffer);
            return buffer;
        }
    }
}
