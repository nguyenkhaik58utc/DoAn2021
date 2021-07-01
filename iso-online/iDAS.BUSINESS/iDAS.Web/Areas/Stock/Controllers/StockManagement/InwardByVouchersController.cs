using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Items;

namespace iDAS.Web.Areas.Stock.Controllers
{
    [BaseSystem(Name = "Bảng kê nhập kho theo chứng từ", Icon = "FolderImage", IsActive = true, IsShow = true, Parent = InwardController.Code, Position = 2)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class InwardByVouchersController : BaseController
    {
        //
        // GET: /Stock/InwardByVouchers/
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
