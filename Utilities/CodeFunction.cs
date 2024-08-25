using System.Security.Cryptography;
using System.Text;

namespace agency_portal_api.Utilities
{
    public static class CodeFunction
    {
        private const string Alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private static readonly Random Random = new Random();

        public static string GenerateToken(int length)
        {
            var _token = GenerateToken(Alphabet, length);
            return GenerateHash(_token);
        }

        private static string GenerateToken(string characters, int length)
        {
            return new string(Enumerable
              .Range(0, length)
              .Select(num => characters[Random.Next() % characters.Length])
              .ToArray());
        }

        private static string GenerateHash(string text)
        {
            using (var hash = SHA256.Create())
            {
                return Convert.ToBase64String(hash.ComputeHash(Encoding.UTF8.GetBytes(text)));
            }
        }
    }
}
