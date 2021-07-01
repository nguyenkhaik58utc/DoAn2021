using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iDAS.Core;
using iDAS.Services;
using iDAS.Items;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Web;
using iDAS.Utilities;
using System.IO;
namespace iDAS.Web.Areas.Document.Controllers
{
    [BaseSystem(Name = "Quản lý danh sách biểu mẫu", Icon = "PageLink", IsActive = true, IsShow = true, Position =1)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class TemplateExportController : BaseController
    {
        //
        // GET: /Document/TemplateExport/
        private TemplateExportSV tempExportSV = new TemplateExportSV();
        private BusinessModuleSV moduleService = new BusinessModuleSV();
        private BusinessFunctionSV functionService = new BusinessFunctionSV();        
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetListTemp(StoreRequestParameters parameters)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var result = tempExportSV.GetAll(filter.PageIndex, filter.PageSize, out filter.PageTotal);
            return this.Store(result);
        }
        public ActionResult LoadModules()
        {
            var modules = moduleService.GetToCombobox();
            return this.Store(modules);
        }
        public ActionResult LoadFunctions(StoreRequestParameters parameters, string moduleCode)
        {
            var functions = functionService.GetFunctionsPermission(moduleCode);
            return this.Store(functions);
        }
        public ActionResult ShowFrmAddTemp()
        {
            var data = new TemplateExportItem();
            string _vName = "Update";
            return new Ext.Net.MVC.PartialViewResult { ViewName = _vName, Model = data };
        }
        
        public ActionResult ShowFrmUpdateTemp(int ID)
        {
            var data = tempExportSV.GetByID(ID);            
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };
        }
        public ActionResult UpdateTemplate(TemplateExportItem data)
        {
            try
            {
                TemplateExportFieldSV tfSV = new TemplateExportFieldSV();
                List<TemplateExportFieldItem> lstFieldItem = new List<TemplateExportFieldItem>();

                var fileUploadField = X.GetCmp<FileUploadField>("FileImportField");
                if (fileUploadField.HasFile)
                {
                    data.File = new FileItem()
                    {
                        ID = Guid.NewGuid(),
                        Name = fileUploadField.FileName,
                        Size = fileUploadField.FileBytes.Length,
                        Type = fileUploadField.PostedFile.ContentType,
                        Extension = string.IsNullOrWhiteSpace(fileUploadField.FileName) ? "" : fileUploadField.FileName.Substring(fileUploadField.FileName.IndexOf('.') + 1),
                        Data = fileUploadField.FileBytes
                    };
                    var direction = Common.FileTempExport + fileUploadField.FileName;
                    string Extension = string.IsNullOrWhiteSpace(fileUploadField.FileName) ? "" : fileUploadField.FileName.Substring(fileUploadField.FileName.IndexOf('.') + 1);
                    if(Extension== "doc" || Extension== "docx")
                    {
                        fileUploadField.PostedFile.SaveAs(Server.MapPath(direction));
                        List<string> lst = ExportWord.readListField(Server.MapPath(direction));
                        foreach (string name in lst)
                        {
                            if (name != "")
                            {
                                int _id = lstFieldItem.Count + 1;
                                lstFieldItem.Add(new TemplateExportFieldItem() { ID = _id, Name = name, Postition = lstFieldItem.Count + 1 });
                            }
                        }
                        System.IO.File.Delete(Server.MapPath(direction));
                    }
                    
                }
                if (data.ID != 0)
                {
                    bool isUpdate = tempExportSV.Update(data, User.ID);
                    if (isUpdate && data.Type==2)
                    {
                        //Xoa het tempField
                        if (tfSV.DeletebyTemID(data.ID))
                        {
                            //Insert lai
                            foreach (var item in lstFieldItem)
                            {
                                item.TemplateExportID = data.ID;
                                tfSV.Insert(item, User.ID);
                            }
                        }

                    }
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
                else
                {
                    int temp_ID = tempExportSV.Insert(data, User.ID);
                    if (temp_ID > 0 && data.Type==2)
                    {
                        foreach (var item in lstFieldItem)
                        {
                            item.TemplateExportID = temp_ID;
                            tfSV.Insert(item, User.ID);
                        }
                    }
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Window>("winEditCate").Close();
                X.GetCmp<Store>("stTempExport").Reload();
            }
            return this.Direct();
        }
        public ActionResult DeleteTemplate(int ID = 0)
        {
            try
            {
                if (ID != 0)
                {
                    tempExportSV.Delete(ID);
                    new TemplateExportFieldSV().DeletebyTemID(ID);
                    Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("stTempExport").Reload();
            }
            return this.Direct();
        }
    }
}
