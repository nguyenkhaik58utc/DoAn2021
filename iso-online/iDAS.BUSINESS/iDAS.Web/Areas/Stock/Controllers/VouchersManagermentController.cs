using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Items;
using iDAS.Services;

namespace iDAS.Web.Areas.Stock.Controllers
{
    [BaseSystem(Name = "Quản lý chứng từ", Icon = "PageWhiteCompressed", IsActive = true, IsShow = true, Position = 5)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class VouchersManagermentController : BaseController
    {
        //
        // GET: /Stock/VouchersManagerment/
        private TransRefSV trans_RefService = new TransRefSV();
        [BaseSystem(Name = "Xem danh sách chứng từ", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetYear()
        {
            List<ComboboxItem> lst = new List<ComboboxItem>();
            for (int i = 2009; i <= DateTime.Now.Year; i++)
            {
                lst.Add(new ComboboxItem
                {
                    ID = i,
                    Name = i.ToString()
                });
            }
            return this.Store(lst);
        }
        public ActionResult GetDataVouchersManagerment(StoreRequestParameters parameters, Nullable<DateTime> fromdate, Nullable<DateTime> todate, string year = "", string choise = "")
        {
            if (fromdate == null && todate == null && year.Equals(""))
            {
                year = DateTime.Now.Year.ToString();
                fromdate = new DateTime(DateTime.Now.Year, 1, 1);
                todate = new DateTime(DateTime.Now.Year, 12, 31);
            }
            List<TransRefItem> lst = new List<TransRefItem>();
            int total;
            lst = trans_RefService.GetForVouchersManagerment(parameters.Page, parameters.Limit, out total, fromdate, todate, year, choise);
            return this.Store(lst, total);
        }
    }
}
