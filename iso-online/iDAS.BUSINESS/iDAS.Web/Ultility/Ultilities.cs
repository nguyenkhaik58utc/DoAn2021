using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Ext.Net;
using iDAS.Core;
using iDAS.Items;
using ISO.API.Models;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace iDAS.Web
{
    public class Ultilities 
    {
        public static string ApplicationJson = "application/json";
        public static string APIServer = ConfigurationManager.AppSettings["APIServer"];
        private static string key = ConfigurationManager.AppSettings["SecurityKey"];

        public static string GenerateJSONWebToken()
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    //issuer: "https://www.yogihosting.com",
                    //audience: "https://www.yogihosting.com",
                    expires: DateTime.Now.AddHours(3),
                    signingCredentials: credentials
                    );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetTokenString()
        {
            var dateTimeOffset = new DateTimeOffset(DateTime.Now);
            //var unixDateTime = dateTimeOffset.ToUnixTimeSeconds();
            var dateTime = DateTime.Now;
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var unixDateTime = (dateTime.ToUniversalTime() - epoch).TotalSeconds;
            var rsa1 = new RSACryptoServiceProvider();

            SignatureModel signature = new SignatureModel()
            {
                signKey = key,
                timestamp = (long)unixDateTime
            };

            byte[] toEncryptData = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(signature));
            TextReader reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "publicKey.xml");
            string publicKey = reader.ReadToEnd();
            reader.Close();
            rsa1.FromXmlString(publicKey);
            byte[] encryptedRSA = rsa1.Encrypt(toEncryptData, false);

            return Convert.ToBase64String(encryptedRSA, 0, encryptedRSA.Length);
        }
    }
}