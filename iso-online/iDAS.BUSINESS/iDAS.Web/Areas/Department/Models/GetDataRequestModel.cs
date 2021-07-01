using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iDAS.Web.Areas.Department.Models
{
    public class GetDataRequestModel
    {
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
    }
}