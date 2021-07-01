using iDAS.Core;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
namespace iDAS.Web.Areas.Stock.Controllers
{
    [BaseSystem(Name = "Bảng kê đóng gói theo chứng từ", Icon = "PageWhiteText", IsActive = true, IsShow = true, Parent = BuildController.Code, Position = 2)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class BuildByVouchersController : BaseController
    {
        //
        // GET: /Stock/BuildByVouchers/
        [BaseSystem(Name = "Xem danh sách", IsActive = true, IsShow = false)]
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
    }
}
