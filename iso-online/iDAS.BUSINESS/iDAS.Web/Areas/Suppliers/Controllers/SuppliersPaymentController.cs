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
using iDAS.Web.Controllers;

namespace iDAS.Web.Areas.Suppliers.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Quản lý công nợ", Icon = "ApplicationSideList", IsActive = true, IsShow = true, Position = 4, Parent = GroupOrdersController.Code)]
    public class SuppliersPaymentController : BaseController
    {
        //
        // GET: /Suppliers/SuppliersPayment/
        private SuppliersOrderSV SuppOrderSV = new SuppliersOrderSV();
        private SuppliersOrderDetailSV SuppOrderDetailSV = new SuppliersOrderDetailSV();
        private SuppliersBillSV suppBillSV = new SuppliersBillSV();
        public static List<SuppliersOrderDetailItem> lstOrderDetail = new List<SuppliersOrderDetailItem>();
        [BaseSystem(Name = "Xem danh sách", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        [BaseSystem(Name = "Xem chi tiết", IsActive = true, IsShow = true)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowDetail(int id)
        {
            var data = suppBillSV.GetById(id);
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "Detail", Model = data };
        }
        [BaseSystem(Name = "Thanh toán", IsActive = true, IsShow = true)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Pay(int id)
        {
            SuppliersBillItem suppBill = new SuppliersBillItem();
            suppBill.SuppliersOrderID = id;
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "Pay", Model = suppBill };
        }
        public ActionResult GetData(StoreRequestParameters parameters)
        {
            return this.Store(lstOrderDetail);
        }
        public ActionResult LoadOrder(StoreRequestParameters parameters)
        {
            int totalCount;
            return this.Store(SuppOrderSV.GetOrderBill(parameters.Page, parameters.Limit, out totalCount), totalCount);
        }
        public ActionResult GetPayListOrder(StoreRequestParameters par, int id)
        {
            var data = new SuppliersBillSV().GetAllbyOrderID(id);
            return this.Store(data);
        }
        [BaseSystem(Name = "Thanh toán", IsActive = true, IsShow = true)]
        public ActionResult ShowFrmPay(int id)
        {
            var data = new SuppliersOrderItem() { ID = id };
            return new Ext.Net.MVC.PartialViewResult { ViewName = "OrderPayList", Model = data };
        }
        [BaseSystem(Name = "Thêm", IsActive = true, IsShow = true)]
        public ActionResult ShowFrm(int id)
        {
            var data = new SuppliersBillItem() { SuppliersOrderID = id };
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Pay", Model = data };
        }

        [BaseSystem(Name = "Sửa", IsActive = true, IsShow = true)]
        public ActionResult UpdateForm(int id = 0)
        {
            var data = suppBillSV.GetById(id);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Pay", Model = data };
        }
        public ActionResult Update(SuppliersBillItem data)
        {

            try
            {
                if (data.ID == 0)
                {
                    suppBillSV.Insert(data, User.ID);                    

                }
                else
                {
                    suppBillSV.Update(data, User.ID);
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
                X.GetCmp<Store>("StorePayListOrder").Reload();
                X.GetCmp<Store>("StoreOrder").Reload();
            }
            return this.Direct();
        }
        [BaseSystem(Name = "Xóa")]
        public ActionResult DeleteRequirement(int id)
        {
            try
            {

                suppBillSV.Delete(id);
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StorePayListOrder").Reload();
                X.GetCmp<Store>("StoreOrder").Reload();
            }
            return this.Direct();
        }

        [ValidateInput(false)]
        public DirectResult ExportExcel(int currentPage,int pageSize)
        {
            //List<SuppliersOrderItem> lst = Ext.Net.JSON.Deserialize<SuppliersOrderItem[]>(arrObject).ToList();
           
            int totalCount=0;
            var lst = SuppOrderSV.GetOrderBill(currentPage, pageSize, out totalCount);

            Dictionary<string, string> dictNameValue = new Dictionary<string, string>() { { "ID", "ID" }, { "CODE", "Mã ĐH" }, { "Name", "Tên đơn hàng" }, { "OrderDate", "Ngày tạo" }, { "PaymentDisp", "HTTT" }, { "Amount", "Tổng tiền HĐ" }, { "AmountRecive", "Đã nhận hàng" }, { "Payed", "Đã TT" }, { "Owe", "Còn Nợ" }, { "StatusDisp", "Trạng thái" } };
            string _title = "Quản lý công nợ";
            ExportToExcel.CreateExcelDocument<SuppliersOrderItem>(lst, "Temp", this.Response, dictNameValue, _title);
            //ExportToExcel.CreateExcelDocument<SuppliersOrderRequirementItem>(lst, "D:\\Temp\\Temp.xlsx", dictNameValue);
            return this.Direct();
        }
    }
}
