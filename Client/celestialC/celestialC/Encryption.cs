using System;
using System.Security.Cryptography;
using System.Text;

namespace celestialC
{
    public static class Encryption
    {
        public static string Base64Encod(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }
        public static string Base64Encode(string plainText)
        {
            return Base64Encod(Base64Encod(Base64Encod(Base64Encod(Base64Encod(plainText)))));
        }
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytesO = System.Convert.FromBase64String(base64EncodedData);
            var base64EncodedBytesT = System.Convert.FromBase64String(Encoding.UTF8.GetString(base64EncodedBytesO));
            var base64EncodedBytesTH = System.Convert.FromBase64String(Encoding.UTF8.GetString(base64EncodedBytesT));
            var base64EncodedBytesF = System.Convert.FromBase64String(Encoding.UTF8.GetString(base64EncodedBytesTH));
            var base64EncodedBytes = System.Convert.FromBase64String(Encoding.UTF8.GetString(base64EncodedBytesF));
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
        public static string ComputeSHA256(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(bytes);
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
