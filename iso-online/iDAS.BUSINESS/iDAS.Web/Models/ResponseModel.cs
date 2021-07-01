using ISO.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Models
{
    public class ResponseModel<T>
    {
        public string MessageCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }
        public int? TotalRows { get; set; }
    }
}
