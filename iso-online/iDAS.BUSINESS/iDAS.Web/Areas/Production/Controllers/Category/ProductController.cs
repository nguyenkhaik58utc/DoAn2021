using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iDAS.Core;
using iDAS.Services;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Web;
using iDAS.Items;
using System.IO;
using iDAS.Utilities;
namespace iDAS.Web.Areas.Production.Controllers
{
    public class ProductController : BaseController
    {
        private ProductSV ProductSV = new ProductSV();
        public ActionResult ProductWindow()
        {
            return new Ext.Net.MVC.PartialViewResult { ViewName = "ProductWindow" };
        }
        public ActionResult LoadProductGroup(StoreRequestParameters parameters)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var result = ProductSV.GetProductGroup(filter);
            return this.Store(result, filter.PageTotal);
        }
        public ActionResult LoadProduct(StoreRequestParameters parameters, int groupId=0)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var result = ProductSV.GetProductByGroup(filter, groupId);
            return this.Store(result, filter.PageTotal);
        }

    }
}
