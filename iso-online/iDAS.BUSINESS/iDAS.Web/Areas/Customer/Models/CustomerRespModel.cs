using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iDAS.Web.Areas.Customer.Models
{
    public class CustomerRespModel
    {
        public int page { get; set; }
        public int pageSize { get; set; }
        public int total { get; set; }
        public List<CustomerItem> lstCus { get; set; }

    }
}