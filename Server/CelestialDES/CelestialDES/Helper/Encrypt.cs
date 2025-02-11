using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CelestialDES.Helper
{
    public static class Encryption
    {
        private const string salt = "zxcbytesecretkeymorg";
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

        public static byte[] Encrypt(byte[] encBytes, string skey)
        {
            char[] key = skey.ToCharArray();
            byte[] newByte = new byte[encBytes.Length];
            int j = 0;
            for (int i = 0; i < encBytes.Length; i++)
            {
                if (j == key.Length)
                {
                    j = 0;
                }
                newByte[i] = (byte)(encBytes[i] ^ Convert.ToByte(key[j]));
                j++;
            }
            return newByte;
        }

        public static byte[] Decompress(byte[] data)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (GZipStream gzipStream = new GZipStream(new MemoryStream(data), CompressionMode.Decompress))
                {
                    gzipStream.CopyTo(memoryStream);
                }

                return memoryStream.ToArray();
            }
        }

        public static byte[] Compress(byte[] data)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (GZipStream gzipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
                {
                    gzipStream.Write(data, 0, data.Length);
                }

                return memoryStream.ToArray();
            }
        }
    }
}
