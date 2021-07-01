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
using iDAS.Utilities;
using iDAS.Web.Controllers;

namespace iDAS.Web.Areas.Suppliers.Controllers
{
    public class SuppliersBidOrdersController : BaseController
    {
        //
        // GET: /Suppliers/SuppliersBidOrders/
        private SuppliersBidOrderSV suppBidOrderSV = new SuppliersBidOrderSV();
        private SupplierSV supplierService = new SupplierSV();
        private SuppliersOrderSV suppOrderSV = new SuppliersOrderSV();
        [BaseSystem(Name = "Xem danh sách", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadSupplier(StoreRequestParameters parameters, string query)
        {
            int total;
            if (string.IsNullOrEmpty(query))
                query = "";
            List<ComboboxItem> lstSupplier = new List<ComboboxItem>();
            lstSupplier = supplierService.Combobox(parameters.Page, parameters.Limit, out total, query);
            return this.Store(lstSupplier, total);
        }
        [BaseSystem(Name = "Xem chi tiết", IsActive = true, IsShow = true)]
        public ActionResult ShowDetail(int id)
        {
            var obj = suppBidOrderSV.GetById(id);
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "Detail", Model = obj };
        }
        [BaseSystem(Name = "Thêm", IsActive = true, IsShow = true)]
        public ActionResult ShowFrm(int id)
        {
            var data = new SuppliersBidOrderItem() { SuppliersOrderID = id };
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };
        }

        public ActionResult ViewRate(int id)
        {
            var data = new SupplierAuditItem() { SupplierID = id };
            return new Ext.Net.MVC.PartialViewResult { ViewName = "FrmSupplierRate",Model = data};
        }
        public ActionResult ShowSuppList()
        {
            return new Ext.Net.MVC.PartialViewResult { ViewName = "FrmFindSuppliers"};
        }
        [BaseSystem(Name = "Sửa", IsActive = true, IsShow = true)]
        public ActionResult UpdateForm(int ID = 0)
        {
            var data = suppBidOrderSV.GetById(ID);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };
        }
        public ActionResult ReciveBidForm(int id = 0)
        {
            var data = suppBidOrderSV.GetById(id);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "ReciveBid", Model = data };
        }
        
        public ActionResult Update(SuppliersBidOrderItem data)
        {

            try
            {
                if (data.ID == 0)
                {
                    var lstSuppSelected = suppBidOrderSV.GetAllByOrderId(data.SuppliersOrderID);
                    if (!lstSuppSelected.Contains(data.SupplierID))
                    {
                        suppBidOrderSV.Insert(data, User.ID);
                        suppOrderSV.UpdateStatus(6, data.SuppliersOrderID);//Cập nhật trạng thái của đơn hàng
                    }
                }
                else
                {
                    suppBidOrderSV.Update(data, User.ID);
                }
                Ultility.CreateNotification(message: RequestMessage.CreateSuccess);

            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Window>("winRequirementListAdd").Close();
                X.GetCmp<Store>("StoreRequirementList").Reload();
            }
            return this.Direct();
        }
        public ActionResult GetListSupp(StoreRequestParameters parameters, int groupSupplierID = 0)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            List<SupplierItem> lstData = new List<SupplierItem>();
            if (groupSupplierID != 0)
            {
                lstData = supplierService.GetAllByGroupID(filter, groupSupplierID);
            }
            return this.Store(lstData, filter.PageTotal);

        }
        public ActionResult InsertSupplierMany(int orderID, string stringId = "0")
        {
            try
            {
                string[] arrayId = stringId.Split(',');
                var data = suppBidOrderSV.GetAllByOrderId(orderID);
                foreach (string suppId in arrayId)
                {
                    int _supID = int.Parse(suppId);
                    if (!data.Contains(_supID))
                    {
                        suppBidOrderSV.Insert(new SuppliersBidOrderItem { SuppliersOrderID = orderID, SupplierID = _supID, Contents = "Yêu cầu báo giá đơn hàng",StartDate=DateTime.Now,EndDate =DateTime.Now.AddDays(5) }, User.ID);                        
                    }
                }
                suppOrderSV.UpdateStatus(6, orderID);//Cập nhật trạng thái của đơn hàng
            }
            catch (Exception ex)
            {
                return this.Direct("Error");
            }
            finally
            {
                X.GetCmp<Store>("StoreRequirementList").Reload();
                X.GetCmp<Window>("frmProducts").Close();
            }
            return this.Direct();
        }     
        [BaseSystem(Name = "Xóa")]
        public ActionResult DeleteRequirement(int id)
        {
            try
            {

                suppBidOrderSV.Delete(id);
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreRequirementList").Reload();
            }
            return this.Direct();
        }

        public ActionResult ReciveBid(SuppliersBidOrderItem data)
        {
            try
            {
                if(data.ID>0)
                {                    
                    data.Status = true;
                    suppBidOrderSV.Update(data, User.ID);
                }                
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreRequirementList").Reload();
                X.GetCmp<Window>("winRequirementListAdd").Close();
            }
            return this.Direct();
        }
        public ActionResult SelectSupplier(int id)
        {
            try
            {
                var obj = suppBidOrderSV.GetById(id);
                if(obj.Status == false)
                {
                    obj.Status = true;
                    suppBidOrderSV.UpdateStatus(obj, User.ID);                    
                }
                suppOrderSV.UpdateSupplier(obj.SupplierID, obj.SuppliersOrderID);
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            finally
            {                
                
                X.GetCmp<Window>("winRequirementList").Close();
                X.GetCmp<Store>("StoreOrder").Reload();
            }
            return this.Direct();
        }
    }
}
