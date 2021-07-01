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
using System.Data;

namespace iDAS.Web.Areas.Suppliers.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Quản lý đơn hàng", Icon = "ApplicationViewColumns", IsActive = true, IsShow = true, Position = 1, Parent = GroupOrdersController.Code)]
    public class SuppliersOrdersManagerController : BaseController
    {
        //
        // GET: /Suppliers/SuppliersOrdersManager/

        private SuppliersOrderSV SuppOrderSV = new SuppliersOrderSV();
        private SuppliersOrderRequirementSV SuppOrderRequiredSV = new SuppliersOrderRequirementSV();
        private SuppliersOrderRequirementDetailSV SuppOrderRequiredDetailSV = new SuppliersOrderRequirementDetailSV();
        private SuppliersOrderDetailSV SuppOrderDetailSV = new SuppliersOrderDetailSV();
        public static List<SuppliersOrderDetailItem> lstOrderDetail = new List<SuppliersOrderDetailItem>();
        public static List<FieldItem> lstFieldItem = new List<FieldItem>();
        public static FileExportItem _data = new FileExportItem();
        public static decimal _amount = 0;
        public int _disc = 0;
        
        [BaseSystem(Name = "Xem danh sách", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetData(StoreRequestParameters parameters)
        {
            return this.Store(lstOrderDetail);
        }
        public ActionResult GetField(StoreRequestParameters parameters)
        {
            return this.Store(lstFieldItem);
        }
        public ActionResult GetListRequired(StoreRequestParameters parameters)
        {
            int totalCount;
            return this.Store(SuppOrderRequiredSV.GetOrderRequirementApproved(parameters.Page, parameters.Limit, out totalCount), totalCount);
        }
        public ActionResult GetOrderDetail(StoreRequestParameters parameters, string orderID)
        {
            int totalCount;
            string[] arrayId = orderID.Split(',');
            var data2 = SuppOrderRequiredDetailSV.GetOrderDetailByID(parameters.Page, parameters.Limit, out totalCount, arrayId);
            return this.Store(data2, totalCount);
        }
        [BaseSystem(Name = "Xem chi tiết", IsActive = true, IsShow = true)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowDetail(int id)
        {
            var obj = SuppOrderSV.GetById(id);
            lstOrderDetail = obj.SuppliersOrderDetails.ToList();
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "Detail", Model = obj };
        }
        public ActionResult LoadOrder(StoreRequestParameters parameters)
        {
            int totalCount;
            return this.Store(SuppOrderSV.GetOrder(parameters.Page, parameters.Limit, out totalCount), totalCount);
        }
        public ActionResult UpdateForm(int id = 0)
        {
            var data = SuppOrderSV.GetById(id);
            lstOrderDetail = data.SuppliersOrderDetails.ToList();
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Insert", Model = data };
        }
        public ActionResult CreateContract(int id = 0)
        {
            var data = SuppOrderSV.GetById(id);
            lstOrderDetail = data.SuppliersOrderDetails.ToList();
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };
        }
        public ActionResult ShowFrmAdd()
        {
            var data = new SuppliersOrderItem();
            string CodeAuto = Common.NextID(SuppOrderSV.GetMaxCode(), "HD");
            data.CODE = CodeAuto;
            lstOrderDetail = new List<SuppliersOrderDetailItem>();
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Insert", Model = data };
        }
        public ActionResult ShowFrmExport(int currentPage, int pageSize, string arrObject)
        {
            FileExportItem data = new FileExportItem();
            data.PageIndex = currentPage;
            data.PageSize = pageSize;            
            lstFieldItem = Ext.Net.JSON.Deserialize<FieldItem[]>(arrObject).ToList();
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Export",Model = data};
        }
        public ActionResult ShowFrmFindProduct()
        {
            return new Ext.Net.MVC.PartialViewResult { ViewName = "FrmFindProducts" };
        }

        public ActionResult GetProducts(string stringId = "0")
        {
            try
            {
                string[] arrayId = stringId.Split(',');
                int rs = lstOrderDetail.Count > 0 ? lstOrderDetail.LastOrDefault().ID : 0;
                var data = SuppOrderDetailSV.GetListProductByStringId(arrayId, rs);
                lstOrderDetail.AddRange(data);
                //X.GetCmp<Store>("stProductOrder").LoadData(lstOrderDetail);
                X.GetCmp<Store>("stProductOrder").Reload();
                X.GetCmp<Window>("frmAuditResult").Close();
                X.GetCmp<GridPanel>("gpProductOder").Refresh();
                return this.Direct();
            }
            catch (Exception ex)
            {
                return this.Direct("Error");
            }

        }
        public ActionResult Update(SuppliersOrderItem data, decimal jjAmount)
        {
            bool success = false;
            try
            {

                if (!checkPrice())
                {
                    Ultility.CreateMessageBox(title: "Thông báo", message: "Bạn phải nhập giá hàng hóa!", icon: MessageBox.Icon.WARNING, button: MessageBox.Button.OK);
                    return this.Direct(false);
                }
                else
                {
                    if (data.ID > 0)
                    {
                        data.Status = 8;
                        data.UpdateAt = DateTime.Now;
                        data.UpdateBy = User.ID;
                        data.Amount = jjAmount;
                        SuppOrderSV.UpdateOrder(data);
                        foreach (SuppliersOrderDetailItem odt in lstOrderDetail)
                        {
                            SuppOrderDetailSV.Update(odt);
                        }
                        Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                        success = true;
                    }

                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreOrder").Reload();
            }
            return this.Direct(success);
        }
        public ActionResult Insert(SuppliersOrderItem data)
        {
            bool success = false;
            try
            {
                if (data.ID > 0)
                {
                    
                        data.UpdateBy = User.ID; data.UpdateAt = DateTime.Now;
                        SuppOrderSV.Update(data);
                        //Xóa và add lại orderdetail
                        SuppOrderDetailSV.DeleteByIdSuppOrderID(data.ID);
                        foreach (SuppliersOrderDetailItem odt in lstOrderDetail)
                        {
                            SuppOrderDetailSV.Insert(odt, data.ID);
                            SuppOrderRequiredDetailSV.UpdateStatus(odt.SuppliersOrderRequirementDetailID.Value, 2);
                        }
                        Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                        success = true;
                    
                }
                else
                {
                    data.CreateAt = DateTime.Now;
                    data.CreateBy = User.ID;
                    data.Status = 4;
                    int _id = SuppOrderSV.Insert(data);
                    foreach (SuppliersOrderDetailItem odt in lstOrderDetail)
                    {
                        SuppOrderDetailSV.Insert(odt, _id);
                        SuppOrderRequiredDetailSV.UpdateStatus(odt.SuppliersOrderRequirementDetailID.Value, 2);
                    }
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                    success = true;
                }

            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.CreateError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreOrder").Reload();
            }
            return this.FormPanel(success);
        }
        public DirectResult HandleChanges(int id, string field, string oldValue, string newValue, string stock_Name, Nullable<int> quantity, Nullable<decimal> Price, Nullable<decimal> amount, object product)
        {
            SuppliersOrderDetailItem obj = lstOrderDetail.FirstOrDefault(i => i.ID == id);
            obj.Price = Price.HasValue ? Price.Value : 0;
            //_amount += amount.HasValue ? amount.Value : 0;
            X.GetCmp<Store>("stProductOrder").GetById(id).Commit();
            X.GetCmp<Store>("stProductOrder").Reload();
            return this.Direct();
        }
        public DirectResult HandleChangesFieldExport(string dataIndex, string text, int position, bool hidden)
        {
            FieldItem obj = lstFieldItem.FirstOrDefault(i => i.dataIndex == dataIndex);
            obj.text = text;
            obj.hidden = hidden;
            if (obj.position > position)
            {
                for (int i = position; i < obj.position; i++)
                {
                    lstFieldItem[i - 1].position++;
                }
                obj.position = position;
                lstFieldItem = lstFieldItem.OrderBy(o => o.position).ToList();
            }
            else if (obj.position < position)
            {
                for (int i = obj.position; i <= position; i++)
                {
                    lstFieldItem[i - 1].position--;
                }
                obj.position = position;
                lstFieldItem = lstFieldItem.OrderBy(o => o.position).ToList();
            }

            X.GetCmp<Store>("stField").Reload();
            return this.Direct();
        }

        private bool checkPrice()
        {
            foreach (SuppliersOrderDetailItem odt in lstOrderDetail)
            {
                if (!odt.Price.HasValue || odt.Price.Value == 0)
                    return false;
            }
            return true;
        }
        public ActionResult DeleteProductInStorage(string stringId = "0")
        {
            try
            {
                string[] arrayId = stringId.Split(',');

                foreach (string sID in arrayId)
                {
                    int id = int.Parse(sID);
                    lstOrderDetail.Remove(lstOrderDetail.FirstOrDefault(i => i.ID == id));
                }

                X.GetCmp<Store>("stProductOrder").Reload();
                return this.Direct();
            }
            catch
            {
                return this.Direct("Error");
            }

        }
        [BaseSystem(Name = "Xóa ", IsActive = true, IsShow = true)]
        [SystemAuthorize(AllowAnonymous = false, CheckAuthorize = true)]
        public ActionResult Delete(string stringId)
        {
            try
            {
                int ids = int.Parse(stringId);
                SuppOrderSV.Delete(ids);
                X.Msg.Notify("Thông báo", "Bạn đã xóa bản ghi thành công!").Show();
                X.GetCmp<Store>("StoreOrder").Reload();
                return this.Direct();
            }
            catch (Exception ex)
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.ERROR,
                    Title = "Thông báo",
                    Message = "Xóa bản ghi không thành công!"
                });
                return this.Direct("Error");
            }
        }
        public DirectResult ExportExcel(FileExportItem data)
        {
            _data = data;
            if (_data.TypeExport == (int)Common.Type_Export.ToFile)
            {
                var fileUploadField = X.GetCmp<FileUploadField>("fileUpload");
                var direction = Common.FileTempExport + Guid.NewGuid().ToString() + ".xlsx";
                if (fileUploadField.HasFile)
                {
                    if (fileUploadField.PostedFile.ContentLength < 300 * 1024 + 1)
                    {
                        
                        if (!System.IO.File.Exists(Server.MapPath(direction)))
                        {
                            fileUploadField.PostedFile.SaveAs(Server.MapPath(direction));
                        }
                        _data._source = Server.MapPath(direction);
                        //ExportToExcel.CreateExcelDocument<SuppliersOrderItem>(lst, Server.MapPath(direction), dictNameValue, Guid.NewGuid().ToString(), this.Response, true);
                    }
                    else
                    {
                        Ultility.CreateMessageBox(title: RequestMessage.Notify, message: "Chỉ cho phép dung lượng import tối đa là 300KB");
                    }
                }
                else
                {
                    Ultility.CreateMessageBox(title: RequestMessage.Notify, message: RequestMessage.NotFileUpload);
                }
            }
            return this.Direct();
        }
        [ValidateInput(false)]
        public ActionResult GetFile()
        {
            int totalCount = 0;
            var lst = SuppOrderSV.GetOrder(_data.PageIndex, _data.PageSize, out totalCount);

            //Dictionary<string, string> dictNameValue = new Dictionary<string, string>() { { "ID", "ID" }, { "CODE", "Mã ĐH" }, { "Name", "Tên đơn hàng" }, { "OrderDate", "Ngày đặt" }, { "ShipDate", "Ngày nhận" }, { "ReciepDate", "Thực tế" }, { "Status", "Trạng thái" } };
            Dictionary<string, string> dictNameValue = new Dictionary<string, string>();
            
            foreach (FieldItem f in lstFieldItem)
            {
                
                if (!f.hidden)
                    dictNameValue.Add(f.dataIndex, f.text);
            }
            if (_data.TypeExport == (int)Common.Type_Export.Default)
                ExportToExcel.CreateExcelDocument<SuppliersOrderItem>(lst, "temp", this.Response, dictNameValue, "Báo cáo ...");
            else
            {                
                 ExportToExcel.CreateExcelDocument<SuppliersOrderItem>(lst, _data._source, dictNameValue,"temp", this.Response, true);
            }          
            return this.Direct();
        }
    }
}
