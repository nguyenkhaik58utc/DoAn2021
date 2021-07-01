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
    [BaseSystem(Name = "Bảng kê xuất kho theo hàng hóa", Icon = "FolderImage", IsActive = true, IsShow = true, Parent = OutwardController.Code, Position = 1)]
    [SystemAuthorize(CheckAuthorize = true)]
    public class OutwardByProductController : BaseController
    {
        //
        // GET: /Stock/OutwardByProduct/
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
