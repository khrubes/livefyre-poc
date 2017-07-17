using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace LiveFrDemo2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public string LiveFyreTokens()
        {
            //FOLLOWING: http://answers.livefyre.com/developers/reference/generating-livefyre-tokens/

            string articleID = "designer-app-1499359797064";
            string articleURL = "http://designer-app.livefyre.com/app/1499359797064";
            string siteKey = "PCwSL8/8OY5DHqeXWOJdm9FBvfQ=";

            /* STEP 1: Build a Dictonary containing the metadata for the collection */

            var meta = new Dictionary<string, object>() {
                    { "title", "poc-comments"}, // ?
                    { "url", articleURL},
                    { "type", "livecomments"}
                };


            /* STEP 2: JSON encode this dictionary, and generate an md5 checksum of it. */

            var metaStr = JsonConvert.SerializeObject(meta);
            byte[] hash;

            using (var md5 = MD5.Create())
            {
                hash = md5.ComputeHash(Encoding.UTF8.GetBytes(metaStr));
            }

            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string 
            for (int i = 0; i < hash.Length; i++)
            {
                sBuilder.Append(hash[i].ToString("x2"));
            }

            string checksum = sBuilder.ToString(); // I'm assuming, not in documentation



            /* STEP 3: Add the articleId to the Dictionary. */

            meta.Add("articleId", articleID);



            /* STEP 4: "Generate a signed JWT Token of the Dictionary, using the Site Key provided to you by Livefyre. 
                Please note that you must specify the correct hash algorithm, as the JWT package does not use HS256 by default." */

            //field JsonWebToken suggested by documentation seems to be deprecated
            //string token = JWT.JsonWebToken.Encode(meta, siteKey, JWT.JwtHashAlgorithm.HS256);

            // workaround for above step
            //https://github.com/jwt-dotnet/jwt 
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            var collectionMetaToken = encoder.Encode(meta, siteKey);

            long epochTicks = new DateTime(1970, 1, 1).Ticks;
            var expiresTime = ((DateTime.Now.AddDays(7).Ticks - epochTicks) / TimeSpan.TicksPerSecond);

            // Create auth token
            var authpayload = new Dictionary<string, object>() {
                { "domain", "microsoft-windows-uat.fyre.co" },
                { "user_id", "user-00001" }, // todo generate unique
                { "expires", expiresTime }, 
                { "display_name", "johndoe" } // ? where do we get this??
            };

            var authToken = encoder.Encode(authpayload, siteKey);

            var result = new Dictionary<string, string>() {
                    { "collectionMetaToken", collectionMetaToken },
                    { "checksum", checksum },
                    { "authToken", authToken }
                };

            return JsonConvert.SerializeObject(result);
        }

    }
}