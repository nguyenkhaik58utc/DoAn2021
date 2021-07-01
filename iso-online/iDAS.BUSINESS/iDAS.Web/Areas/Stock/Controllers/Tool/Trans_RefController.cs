using Ext.Net;
using Ext.Net.MVC;
using iDAS.Web.Areas.Stock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iDAS.Core;
using iDAS.DA;
using iDAS.Items;
using iDAS.Services;
namespace iDAS.Web.Areas.Stock.Controllers
{
    [BaseSystem(Name = "Lịch sử hàng hóa", Icon = "FolderImage", IsActive = true, IsShow = true, Parent = ToolController.Code)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class Trans_RefController : BaseController
    {
        private TransRefSV trans_RefService = new TransRefSV();
        private StockProductSV productService = new StockProductSV();
        //
        // GET: /Stock/Trans_Ref/
        [BaseSystem(Name = "Xem danh sách", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetDataTrans_Ref(StoreRequestParameters parameters, Nullable<DateTime> fromdate, Nullable<DateTime> todate, Nullable<int> productId, string year = "")
        {
            if (fromdate == null && todate == null && year.Equals(""))
            {
                year = DateTime.Now.Year.ToString();
                fromdate = new DateTime(DateTime.Now.Year, 1, 1);
                todate = new DateTime(DateTime.Now.Year, 12, 31);
            }
            if (productId == null)
            {
                productId = 0;
            }
            List<InventoryDetailItem> lst = new List<InventoryDetailItem>();
            int total;
            lst = trans_RefService.GetAll(parameters.Page, parameters.Limit, out total, fromdate, todate, productId, year);
            return this.Store(lst, total);
        }
        public ActionResult LoadProduct(int start, int limit, int page, string query)
        {
            if (string.IsNullOrEmpty(query))
                query = "";
            List<ComboboxItem> lstproduct = productService.Combobox(query);
            Paging<ComboboxItem> plants = new Paging<ComboboxItem>(lstproduct, lstproduct.Count);
            return this.Store(plants.Data, plants.TotalRecords);
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
