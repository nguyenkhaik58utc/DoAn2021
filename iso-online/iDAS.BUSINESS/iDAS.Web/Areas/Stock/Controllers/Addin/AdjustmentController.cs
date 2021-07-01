using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Core;
using iDAS.Items;
using iDAS.Utilities;
using iDAS.Services;

namespace iDAS.Web.Areas.Stock.Controllers
{
    [BaseSystem(Name = "Kiểm kê", Icon = "Tick", IsActive = true, IsShow = true, Parent = AddinController.Code, Position = 7)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class AdjustmentController : BaseController
    {
        private AdjustmentSV adjustmentService = new AdjustmentSV();
        private QualityNCSV NCSV = new QualityNCSV();
        public ActionResult Index()
        {
            AdjustmentDetailSV.Clear();
            return View();
        }
        public DirectResult HandleChanges(int id, string field, string oldValue, string newValue, Nullable<decimal> currentQty, object product)
        {
            AdjustmentDetailItem obj = new AdjustmentDetailItem();
            obj.ID = id;
            if (field.Equals("UnitPrice"))
            {
                obj.UnitPrice = decimal.Parse(newValue);
            }
            else if (field.Equals("NewQty"))
            {
                obj.NewQty = decimal.Parse(newValue);
                obj.QtyDiff = decimal.Parse(newValue) - currentQty;
            }
            AdjustmentDetailSV.UpdateAdjustmentDetail(obj);
            return this.Direct();
        }
        [BaseSystem(Name = "Thêm", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Insert(int StockID)
        {
            try
            {
                AdjustmentDetailSV.Clear();
                var data = adjustmentService.GetByStockId(StockID);
                List<AdjustmentDetailItem> lsttemp = new List<AdjustmentDetailItem>();
                if (data != null && data.Count > 0)
                {
                    foreach (var item in data)
                    {
                        lsttemp.Add(new AdjustmentDetailItem
                         {
                             ID = item.ID,
                             StockProductID = (int)item.StockProductID,
                             Product_Code = item.ProductCode,
                             Amount = item.Amount,
                             Batch = item.Batch,
                             CurrentQty = item.Quantity,
                             Description = item.Description,
                             Orgin = item.Orgin,
                             Group_Name = item.Group_Name,
                             Product_Name = item.ProductName,
                             StockID = item.StockID,
                             Unit = item.Unit,
                             UnitPrice = item.UnitPrice
                         });

                    }
                }
                List<AdjustmentDetailItem> lst = new List<AdjustmentDetailItem>();
                lst.AddRange(lsttemp);
                AdjustmentDetailSV.AddAdjustmentDetail(lst);
                X.GetCmp<Store>("stAdjustment").LoadData(AdjustmentDetailSV.Storage);
                X.GetCmp<Store>("stAdjustment").Reload();
                X.GetCmp<GridPanel>("gpAdjustment").Refresh();
                return this.Direct();
            }
            catch
            {
                return this.Direct("Error");
            }
        }
        public ActionResult GetData(Nullable<int> StockID)
        {
            var data = AdjustmentDetailSV.Storage;
            if (StockID == null)
            {
                StockID = 1;
            }
            return this.Store(data);
        }
        public ActionResult GetDataDetail(StoreRequestParameters parameters)
        {
            List<AdjustmentItem> lst;
            int total;
            lst = adjustmentService.GetAll(parameters.Page, parameters.Limit, out total);
            return this.Store(new Paging<AdjustmentItem>(lst, total));
        }
        public ActionResult GetDataForDetail(StoreRequestParameters parameters, int idAdjustment = 0)
        {

            List<AdjustmentDetailItem> lst;
            int total;
            lst = adjustmentService.GetAll(parameters.Page, parameters.Limit, out total, idAdjustment);
            return this.Store(new Paging<AdjustmentDetailItem>(lst, total));
        }
        public ActionResult InsertAdjustment(int StockID, Nullable<decimal> amount)
        {
            try
            {
                int temp = adjustmentService.GetAdjustmentOfCurrentDay(StockID);
                AdjustmentItem objNew = new AdjustmentItem();
                var data = AdjustmentDetailSV.Storage;
                List<AdjustmentDetailItem> lst = new List<AdjustmentDetailItem>();
                if (temp == 0)
                {
                    objNew.Active = true;
                    objNew.Amount = amount;
                    objNew.CreateAt = DateTime.Now;
                    objNew.CreateBy = User.ID;
                    objNew.RefDate = DateTime.Now;
                    objNew.RefType = (int)Common.RefType.adjustment;
                    objNew.Employee_ID = User.ID;
                    objNew.Accept = true;
                    objNew.Description = "(Kiểm kê)";
                    objNew.StockID = StockID;
                    objNew.IsClose = false;
                    objNew.Ref_OrgNo = Common.NextID(adjustmentService.GetMaxCode(), "KK");
                    int adjustmentId = adjustmentService.Insert(objNew);
                    foreach (var item in data)
                    {
                        lst.Add(new AdjustmentDetailItem()
                        {
                            StockProductID = item.StockProductID,
                            Product_Name = item.Product_Name,
                            UnitConvert = 1,
                            UnitPrice = item.UnitPrice,
                            QtyConvert = item.UnitPrice,
                            StoreID = 0,
                            Description = "(Kiểm kê)",
                            CreateAt = DateTime.Now,
                            CreateBy = User.ID,
                            StockAdjustmentID = adjustmentId,
                            Amount = item.Amount,
                            CurrentQty = item.CurrentQty,
                            NewQty = item.NewQty,
                            QtyDiff = item.QtyDiff,
                            StockID = StockID,
                            Unit_ID = item.Unit_ID
                        });
                    }
                }
                else
                {
                    adjustmentService.Update(temp, amount);
                    foreach (var item in data)
                    {
                        lst.Add(new AdjustmentDetailItem()
                        {
                            StockProductID = item.StockProductID,
                            Product_Name = item.Product_Name,
                            UnitConvert = 1,
                            UnitPrice = item.UnitPrice,
                            QtyConvert = item.UnitPrice,
                            StoreID = 0,
                            Description = "(Kiểm kê)",
                            CreateAt = DateTime.Now,
                            CreateBy = User.ID,
                            StockAdjustmentID = temp,
                            Amount = item.Amount,
                            CurrentQty = item.CurrentQty,
                            NewQty = item.NewQty,
                            QtyDiff = item.QtyDiff,
                            StockID = StockID,
                            Unit_ID = item.Unit_ID

                        });
                    }
                    adjustmentService.Delete(temp);
                }
                adjustmentService.Insert(lst);
                AdjustmentDetailSV.Clear();
                X.GetCmp<Store>("stAdjustment").Reload();
                X.GetCmp<NumberField>("txtAmount").Reset();
                X.Msg.Notify("Thông báo", "Bạn đã thêm một phiếu kiểm kê thành công!").Show();
                return this.Direct();
            }
            catch
            {
                return this.Direct("Error");
            }
        }
        [BaseSystem(Name = "Xóa", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Delete(int id)
        {
            adjustmentService.Close(id);
            X.Msg.Notify("Thông báo", "Bạn đã xóa bản ghi thành công!").Show();
            X.GetCmp<Store>("stAdjustmentDetail").Reload();
            X.GetCmp<GridPanel>("gpAdjustmentDetail").Refresh();
            return this.Direct();
        }
        [BaseSystem(Name = "Xem chi tiết", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmDetail(int idAdjustment)
        {
            try
            {
                var obj = adjustmentService.GetById(idAdjustment);
                return new Ext.Net.MVC.PartialViewResult { ViewName = "ViewDetail", ViewData = new ViewDataDictionary { { "idAdjustment", idAdjustment }, { "adjustmentCode", obj.Ref_OrgNo } } };
            }
            catch
            {
                return this.Direct("Error");
            }
        }
        [BaseSystem(Name = "Thêm sự không phù hợp", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult NCShow(int adjustmentId)
        {
            var data = new QualityNCItem();
            data = NCSV.GetByAdjustment(adjustmentId);
            ViewData["adjustmentId"] = adjustmentId;
            return new Ext.Net.MVC.PartialViewResult
            {
                ViewName = data.ID != 0 ? (data.IsVerify ? "DetailView" : "UpdateView") : "NCCommon",
                ViewData = ViewData,
                Model = data,
            };
        }
        public ActionResult AuditNCUpdate(QualityNCItem item, int adjustmentId = 0)
        {
            bool success = false;
            try
            {
                if (item.ID != 0)
                {
                    NCSV.Update(item, User.ID);
                }
                else
                {
                    item.CreateBy = User.ID;
                    NCSV.InsertFromAdjustment(item, adjustmentId);
                }
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                success = true;
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
                throw;
            }
            return this.FormPanel(success);
        }
    }
}
