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
using System.Xml;
using System.Xml.XPath;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Xml.Xsl;
using System.Runtime.Serialization;
using System.Text;

namespace iDAS.Web.Areas.Suppliers.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Bảng kê đơn hàng", Icon = "ApplicationSideList", IsActive = true, IsShow = true, Position = 3, Parent = GroupOrdersController.Code)]
    public class SuppliersOrderListController : BaseController
    {
        //
        // GET: /Suppliers/SuppliersOrderList/
        private SuppliersOrderSV SuppOrderSV = new SuppliersOrderSV();
        private SuppliersOrderDetailSV SuppOrderDetailSV = new SuppliersOrderDetailSV();
        public static List<SuppliersOrderDetailItem> lstOrderDetail = new List<SuppliersOrderDetailItem>();
        public static List<FieldItem> lstFieldItem = new List<FieldItem>();
        public static FileExportItem _data = new FileExportItem();
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
            var obj = SuppOrderSV.GetById(id);
            lstOrderDetail = obj.SuppliersOrderDetails.ToList();
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "DetailRecive", Model = obj };
        }
        public ActionResult UpdateRecive(int id)
        {
            var obj = SuppOrderSV.GetById(id);
            lstOrderDetail = obj.SuppliersOrderDetails.ToList();
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "UpdateRecive", Model = obj };
        }
        public ActionResult DetailRecive(int id)
        {
            var obj = SuppOrderSV.GetById(id);
            lstOrderDetail = obj.SuppliersOrderDetails.ToList();            
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "DetailRecive", Model = obj };
        }
        public ActionResult Update(SuppliersOrderItem data)
        {
            bool success = false;
            try
            {   
                    if (data.ID > 0)
                    {
                        data.Status = 9;
                        data.UpdateAt = DateTime.Now;
                        data.UpdateBy = User.ID;
                        
                        SuppOrderSV.UpdateOrderRecive(data);
                        foreach (SuppliersOrderDetailItem odt in lstOrderDetail)
                        {
                            SuppOrderDetailSV.UpdateRecive(odt);
                        }
                        Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                        success = true;
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
            return this.FormPanel(success);
        }
        public ActionResult GetData(StoreRequestParameters parameters)
        {
            return this.Store(lstOrderDetail);
        }
        public ActionResult LoadOrder(StoreRequestParameters parameters)
        {
            int totalCount;
            return this.Store(SuppOrderSV.GetOrderList(parameters.Page, parameters.Limit, out totalCount), totalCount);
        }
        public DirectResult HandleChanges(int id, string field, string oldValue, string newValue, string stock_Name, Nullable<int> ReciveQuantity, Nullable<int> ReciveQuality,string note, object product)
        {
            SuppliersOrderDetailItem obj = lstOrderDetail.FirstOrDefault(i => i.ID == id);
            obj.ReciveQuality = ReciveQuality.HasValue ? ReciveQuality.Value : 0;
            obj.ReciveQuantity = ReciveQuantity.HasValue ? ReciveQuantity.Value : 0;
            obj.Note = note;
            //_amount += amount.HasValue ? amount.Value : 0;
            X.GetCmp<Store>("stProductOrder").GetById(id).Commit();
            X.GetCmp<Store>("stProductOrder").Reload();
            return this.Direct();
        }
        public ActionResult ShowFrmExport(int currentPage, int pageSize, string arrObject)
        {
            FileExportItem data = new FileExportItem();
            data.PageIndex = currentPage;
            data.PageSize = pageSize;
            lstFieldItem = Ext.Net.JSON.Deserialize<FieldItem[]>(arrObject).ToList();
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Export", Model = data };
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
            var lst = SuppOrderSV.GetOrderList(_data.PageIndex, _data.PageSize, out totalCount);
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
                ExportToExcel.CreateExcelDocument<SuppliersOrderItem>(lst, _data._source, dictNameValue, Guid.NewGuid().ToString(), this.Response, true);
            }
            return this.Direct();
        }
        public ActionResult GetField(StoreRequestParameters parameters)
        {
            return this.Store(lstFieldItem);
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
    }
}
