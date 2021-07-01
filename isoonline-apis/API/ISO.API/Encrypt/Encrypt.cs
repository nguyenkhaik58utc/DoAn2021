using ISO.API.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ISO.API.Encrypt
{
    public class Encrypt
    {
        public Encrypt()
        {

        }
        static public byte[] Encryption(byte[] Data, RSAParameters RSAKey, bool DoOAEPPadding)
        {
            try
            {
                byte[] encryptedData;
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    RSA.ImportParameters(RSAKey);
                    encryptedData = RSA.Encrypt(Data, DoOAEPPadding);
                }
                return encryptedData;
            }
            catch (CryptographicException e)
            {
                return null;
            }

        }

        static public byte[] Decryption(byte[] Data, RSAParameters RSAKey, bool DoOAEPPadding)
        {
            try
            {
                byte[] decryptedData;
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    RSA.ImportParameters(RSAKey);
                    decryptedData = RSA.Decrypt(Data, DoOAEPPadding);
                }
                return decryptedData;
            }
            catch (CryptographicException e)
            {
                return null;
            }

        }

        public static void WriteRSAInfoToFile()
        {
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
            TextWriter writer = new StreamWriter(System.AppContext.BaseDirectory + "publicKey.xml");
            string publicKey = RSA.ToXmlString(false);
            writer.Write(publicKey);
            writer.Close();

            writer = new StreamWriter(System.AppContext.BaseDirectory + "privateKey.xml");
            string privateKey = RSA.ToXmlString(true);
            writer.Write(privateKey);
            writer.Close();
        }

        public static bool CheckSignature(string signature)
        {
            try
            {
                var rsa = new RSACryptoServiceProvider();
                TextReader reader = new StreamReader(System.AppContext.BaseDirectory + "privateKey.xml");
                string privateKey = reader.ReadToEnd();
                reader.Close();
                rsa.FromXmlString(privateKey);

                byte[] decryptedRSA = rsa.Decrypt(System.Convert.FromBase64String(signature), false);
                string originalResult = Encoding.Default.GetString(decryptedRSA);
                SignatureModel signatureModel = JsonConvert.DeserializeObject<SignatureModel>(originalResult);

                var signKey = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Jwt")["SigningSecret"];
                var timeStamp = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Jwt")["TimeStamp"];

                var localDateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(signatureModel.timestamp).DateTime.ToLocalTime();
                var countTime = DateTime.Now.Subtract(localDateTimeOffset).TotalSeconds;
                if (signatureModel.signKey != signKey || countTime > Convert.ToDouble(timeStamp))
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
