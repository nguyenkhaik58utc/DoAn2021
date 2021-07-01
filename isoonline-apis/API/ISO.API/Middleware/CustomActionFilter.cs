using ISO.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ISO.API.Middleware
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RsaAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                if (context.HttpContext.Request.Method == "GET"|| context.HttpContext.Request.Method == "DELETE")
                { 
                    Microsoft.Extensions.Primitives.StringValues signature;
                    var header = context.HttpContext.Request.Headers.TryGetValue("signature", out signature);
                    var check = Encrypt.Encrypt.CheckSignature(signature.ToString());
                    if (!check)
                        context.Result = new CustomUnauthorizedResult("Authorization failed.", "0002");
                }
                else
                {
                    var bodyStr = "";
                    var req = context.HttpContext.Request;
                    // Allows using several time the stream in ASP.Net Core
                    req.EnableBuffering();
                    req.Body.Position = 0;
                    // Arguments: Stream, Encoding, detect encoding, buffer size 
                    // AND, the most important: keep stream opened
                    using (StreamReader reader = new StreamReader(req.Body, Encoding.UTF8, true, 1024, leaveOpen: true))
                    {
                        bodyStr = reader.ReadToEnd();
                    }
                    // Rewind, so the core is not lost when it looks the body for the request
                    req.Body.Position = 0;
                    RequestModel<object> model = JsonConvert.DeserializeObject<RequestModel<object>>(bodyStr);
                    var check = Encrypt.Encrypt.CheckSignature(model.Signature);
                    if (!check)
                        context.Result = new CustomUnauthorizedResult("Authorization failed.", "0002");
                }
            }
            catch (Exception ex)
            {
                context.Result = new CustomUnauthorizedResult(ex.Message.ToString(), "0002");
            }
        }
    }
    public class CustomUnauthorizedResult : JsonResult
    {
        public CustomUnauthorizedResult(string message, string messageCode) : base(new CustomError(message, messageCode))
        {
            StatusCode = StatusCodes.Status401Unauthorized;
        }
    }
    public class CustomError
    {
        public string Message { get; }
        public string MessageCode { get; }

        public CustomError(string message, string messageCode)
        {
            Message = message;
            MessageCode = messageCode;
        }
    }
}
