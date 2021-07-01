using Ext.Net;
using Ext.Net.MVC;
using iDAS.Core;
using iDAS.Items;
using iDAS.Services;
using iDAS.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Generic.Controllers
{
    public class ExportExcelFileController : BaseController
    {
        //
        // GET: /Generic/ExportExcelFile/
        public static List<FieldItem> lstFieldItem = new List<FieldItem>();
        public static FileExportItem _data = new FileExportItem();
        public static string lstData;
        public ActionResult Index()
        {
            return View();
        }
        [ValidateInput(false)]
        public ActionResult ExportWindow(string arrObject,string arrData,int currentPage = 1, int pageSize=30)
        {
            try
            {
                FileExportItem data = new FileExportItem();
                data.PageIndex = currentPage;
                data.PageSize = pageSize;
                lstFieldItem = new List<FieldItem>();
                lstFieldItem = Ext.Net.JSON.Deserialize<FieldItem[]>(arrObject).ToList();
                lstData = arrData;
                return new Ext.Net.MVC.PartialViewResult
                {
                    Model = data
                };
            }
            catch
            {
                return new Ext.Net.MVC.PartialViewResult
                {
                    Model = "<html><body><div style='text-align:center; width:100%; font-weight:bold'>Chức năng đang xây dựng</div><body></html>",
                };
            }
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
            Dictionary<string, string> dictNameValue = new Dictionary<string, string>();
            foreach (FieldItem f in lstFieldItem)
            {
                if (!f.hidden)
                    dictNameValue.Add(f.dataIndex, f.text);
            }
            List<Dictionary<string, string>> lst = Ext.Net.JSON.Deserialize<Dictionary<string, string>[]>(lstData).ToList();
            if (_data.TypeExport == (int)Common.Type_Export.Default)
                ExportToExcel.CreateExcelDocument_obj(lst, "temp", this.Response, dictNameValue, _data.Title);
            else
                ExportToExcel.CreateExcelDocument_obj(lst, _data._source, dictNameValue, "temp", this.Response, true);
            return this.Direct();
        }

        [ValidateInput(false)]
        public ActionResult ExportExcelFile(string title, string functionCode,string arrObject, string arrData)
        {
            var TempExp = new TemplateExportSV().GetFileByFunctionCode(functionCode);
            try
            {
                lstFieldItem = new List<FieldItem>();
                lstFieldItem = Ext.Net.JSON.Deserialize<FieldItem[]>(arrObject).ToList();
                Dictionary<string, string> dictNameValue = new Dictionary<string, string>();
                foreach (FieldItem f in lstFieldItem)
                {
                    if (!f.hidden && !string.IsNullOrEmpty(f.dataIndex))
                        dictNameValue.Add(f.dataIndex, f.text);
                }
                List<Dictionary<string, string>> lst = Ext.Net.JSON.Deserialize<Dictionary<string, string>[]>(arrData).ToList();
                
                if (TempExp != null && TempExp.File.Data.Length > 0)
                {
                    System.IO.File.WriteAllBytes(Server.MapPath(Common.FileTempExport + TempExp.FileName), TempExp.File.Data);
                    ExportToExcel.CreateExcelDocument_obj(lst, Server.MapPath(Common.FileTempExport + TempExp.FileName), dictNameValue, TempExp.Name, this.Response, true, true);
                    new FileInfo(Server.MapPath(Common.FileTempExport + TempExp.FileName)).Delete();
                }
                else
                    ExportToExcel.CreateExcelDocument_obj(lst, "temp", this.Response, dictNameValue, title);
                Ultility.CreateNotification(message: RequestMessage.ExportSucess);
            }catch (Exception ex)
            {
                Ultility.CreateNotification(message:RequestMessage.ExportSucess +"--"+ ex.Message, error: true);
            }
            return this.Direct();
        }
    }
}
