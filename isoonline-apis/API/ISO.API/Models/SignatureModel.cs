using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Models
{
    public class SignatureModel
    {
        public string signKey { get; set; }
        public long timestamp { get; set; }
    }
}
