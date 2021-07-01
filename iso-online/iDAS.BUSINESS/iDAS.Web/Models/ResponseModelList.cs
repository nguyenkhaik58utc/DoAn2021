using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iDAS.Web.Models
{
    public class ResponseModelList<T> where T : class
    {
        public string messageCode { get; set; }
        public string message { get; set; }
        public List<T> Data { get; set; }
    }
}