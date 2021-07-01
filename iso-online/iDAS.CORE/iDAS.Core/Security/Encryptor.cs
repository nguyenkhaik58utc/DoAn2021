using System;
using System.Security.Cryptography;
using System.Text;

namespace iDAS.Core
{
    public class Encryptor
    {
        public static string Encrypt(string value)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(value);
            byte[] data = HashAlgorithm.Create("SHA1").ComputeHash(bytes);
            return Convert.ToBase64String(data);
        }

        public static string Encode(string value)
        {
            var data = Encoding.Unicode.GetBytes(value);
            return Convert.ToBase64String(data);
        }

        public static string Decode(string value)
        {
            // HungNM. Fix error "Invalid length for a Base-64 char array or string". 20200916.
            int mod4 = value.Length % 4;
            if (mod4 > 0)
            {
                value += new string('=', 4 - mod4);
            }
            // End. HungNM.

            var data = Convert.FromBase64String(value);
            return Encoding.Unicode.GetString(data);
        }

        public static Guid CreateGUID(string value)
        {
            byte[] data = MD5.Create().ComputeHash(Encoding.Default.GetBytes(value));
            return new Guid(data);
        }
    }
}
