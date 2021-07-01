using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Threading.Tasks;

namespace ISO.API.Models
{
    public class RequestModel <T> where T : class
    {
        public T Data { get; set; }
        public string Signature { get; set; }
    }
}
