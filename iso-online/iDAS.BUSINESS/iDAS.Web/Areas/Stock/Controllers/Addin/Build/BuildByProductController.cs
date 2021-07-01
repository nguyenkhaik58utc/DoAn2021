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
    [BaseSystem(Name = "Bảng kê đóng gói theo hàng hóa", Icon = "PageWhiteText", IsActive = true, IsShow = true, Parent = BuildController.Code, Position = 1)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class BuildByProductController : BaseController
    {
        //
        // GET: /Stock/BuildByProduct/
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
