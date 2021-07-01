using Ext.Net;
using iDAS.Web.Areas.Stock;
using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net.MVC;
using iDAS.DA;
using iDAS.Items;
using iDAS.Services;

namespace iDAS.Web.Areas.Stock.Controllers
{
    [BaseSystem(Name = "Tồn kho", Icon = "FolderImage", IsActive = true, IsShow = true, Parent = GroupManageStockController.Code, Position = 4)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class InventoryController : BaseController
    {
        private InventorySV inventoryService = new InventorySV();
        private StockSV listStockService = new StockSV();
        //
        // GET: /Stock/Inventory/       
        [BaseSystem(Name = "Xem tồn kho", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadStock(int start, int limit, int page, string query)
        {
            if (string.IsNullOrEmpty(query))
                query = "";
            List<ComboboxItem> lstStock = listStockService.Combobox(query);
            Paging<ComboboxItem> plants = new Paging<ComboboxItem>(lstStock, lstStock.Count);
            return this.Store(plants.Data, plants.TotalRecords);
        }
        public ActionResult GetDataInventory(StoreRequestParameters parameters, Nullable<int> StockID, int method)
        {
            if (StockID == null)
            {
                StockID = 0;
            }
            List<InventoryItem> lst = new List<InventoryItem>();
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            lst = inventoryService.GetAll(filter, StockID, method);
            return this.Store(lst, filter.PageTotal);
        }
        public ActionResult GetDataReportByGroupProduct(StoreRequestParameters parameters, Nullable<int> StockID, Nullable<int> group_Id, int method)
        {
            if (StockID == null)
            {
                StockID = 0;
            }
            if (group_Id == null)
            {
                group_Id = 0;
            }
            List<InventoryItem> lst = new List<InventoryItem>();
            int total;
            lst = inventoryService.GetAllReportByGroupProduct(parameters.Page, parameters.Limit, out total, StockID, group_Id, method);
            return this.Store(lst, total);
        }
        public ActionResult GetDataReportByQuantity(StoreRequestParameters parameters, Nullable<int> StockID, Nullable<DateTime> fromDate, Nullable<DateTime> toDate)
        {
            if (StockID == null)
            {
                StockID = 1;
            }
            List<InventoryItem> lst = new List<InventoryItem>();
            int total;
            lst = inventoryService.GetAllReportByQuantity(parameters.Page, parameters.Limit, out total, StockID, fromDate, toDate);
            return this.Store(lst, total);
        }
        public ActionResult LoadUnit(int stockId = 0, int productId = 0)
        {
            var data = new List<UnitStockItem>();
            data = inventoryService.GetUnitStock(stockId, productId);
            return this.Store(data);
        }
    }
}
