using iDAS.Items;
using iDAS.Services;
using iDAS.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using iDAS.Core;

namespace iDAS.Web.Areas.Generic.Controllers
{
    public class ExportWordFileController : BaseController
    {
        //
        // GET: /Generic/ExportWordFile/
        private TemplateExportSV tempExportSV = new TemplateExportSV();
        private CustomerContractSV CustomerContractService = new CustomerContractSV();
        static List<TemplateExportFieldItem> lstFieldItem ;
        static Dictionary<string, string> dictFieldValue;
        static FileItem fileItem;
        public ActionResult ExportWordFile(string functionCode, string moduleCode, string arrData)
        {
            var tempExp = tempExportSV.GetFileByFunctionCode(functionCode, 2);
            if(tempExp!=null)
            {
                var direction = Common.FileTempExport + "temp" + DateTime.UtcNow.Ticks.ToString() + ".docx";
                var path = Server.MapPath(direction);
                //var data = ExportWord.ItemToDictionary<CustomerContractItem>(CustomerContractService.GetById(ContractID));
                var data = JSON.Deserialize<KeyValuePair<string, string>[]>(arrData);
                Dictionary<string, string> dict = new Dictionary<string, string>();
                foreach (KeyValuePair<string, string> pair in data)
                {
                    dict.Add(pair.Key, pair.Value);
                }
                var lstTempField = new TemplateExportFieldSV().GetAllByTemID(tempExp.ID).OrderBy(m => m.Postition.Value).ToList();
                var filetest = new FileSV().GetById(tempExp.FileID.Value);
                System.IO.File.WriteAllBytes(Server.MapPath(Common.FileTempExport + tempExp.FileName), filetest.Data);
                EditTag(Server.MapPath(Common.FileTempExport + tempExp.FileName), dict, lstTempField);
                ExportWord.dowloadFile(Server.MapPath(Common.FileTempExport + tempExp.FileName), this.Response, tempExp.Name);
                return this.Direct();
            }else
            {
                var data = new TemplateExportItem();
                data.FunctionCode = functionCode;
                data.ModuleCode = moduleCode;
                var data1 = JSON.Deserialize<KeyValuePair<string, string>[]>(arrData);
                dictFieldValue = new Dictionary<string, string>();
                foreach (KeyValuePair<string, string> pair in data1)
                {
                    dictFieldValue.Add(pair.Key, pair.Value);
                }
                return new Ext.Net.MVC.PartialViewResult { ViewName = "InsertTempl", Model = data };
            }
            
            //return File(tempExp.File.Data, "application/force-download");
        }
        private void EditTag(string path, Dictionary<string, string> dictValue, List<TemplateExportFieldItem> lstTempField)
        {
            try
            {
                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(path, true))
                {
                    var maintPart = wordDoc.MainDocumentPart;
                    foreach (TemplateExportFieldItem item in lstTempField)
                    {
                        if (!string.IsNullOrEmpty(item.Field) && dictValue.ContainsKey(item.Field))
                        {
                            var res = from bm in maintPart.Document.Body.Descendants()
                                      where bm.InnerText != string.Empty && bm.InnerText== item.Name && bm.HasChildren == false
                                      select bm;
                            foreach (var i in res)
                            {
                                i.InsertAfterSelf(new Text(i.InnerText.Replace(item.Name, dictValue[item.Field])));
                                i.Remove();
                            }
                        }
                        else
                        {
                            var res = from bm in maintPart.Document.Body.Descendants()
                                      where bm.InnerText != string.Empty && bm.InnerText==item.Name && bm.HasChildren == false
                                      select bm;
                            foreach (var i in res)
                            {
                                i.InsertAfterSelf(new Text(i.InnerText.Replace(item.Name, "")));
                                i.Remove();
                            }
                        }

                    }
                    var res1 = from bm in maintPart.Document.Body.Descendants()
                               where bm.InnerText != string.Empty && bm.InnerText.Contains("<tag>") && bm.HasChildren == false
                               select bm;
                    foreach (var i in res1)
                    {
                        i.InsertAfterSelf(new Text(i.InnerText.Replace("<tag>", "")));
                        i.Remove();
                    }
                    var res2 = from bm in maintPart.Document.Body.Descendants()
                               where bm.InnerText != string.Empty && bm.InnerText.Contains("</tag>") && bm.HasChildren == false
                               select bm;
                    foreach (var i in res2)
                    {
                        i.InsertAfterSelf(new Text(i.InnerText.Replace("</tag>", "")));
                        i.Remove();
                    }
                    maintPart.Document.Save();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult SelectImportFile()
        {
            var fileUploadField = X.GetCmp<FileUploadField>("FileImportField");
            var direction = Common.FileTempExport + Guid.NewGuid().ToString() + ".docx";
            if (fileUploadField.HasFile)
            {
                if (fileUploadField.PostedFile.ContentLength < 300 * 1024 + 1)
                {

                    if (!System.IO.File.Exists(Server.MapPath(direction)))
                    {
                        fileUploadField.PostedFile.SaveAs(Server.MapPath(direction));
                        List<string> lst = ExportWord.readListField(Server.MapPath(direction));
                        lstFieldItem = new List<TemplateExportFieldItem>();
                        foreach (string name in lst)
                        {
                            if (name != "")
                            {
                                int _id = lstFieldItem.Count + 1;
                                lstFieldItem.Add(new TemplateExportFieldItem() { ID = _id, Name = name, Postition = lstFieldItem.Count + 1 });
                            }
                        }
                        fileItem = new FileItem()
                        {
                            ID = Guid.NewGuid(),
                            Name = fileUploadField.FileName,
                            Size = fileUploadField.FileBytes.Length,
                            Type = fileUploadField.PostedFile.ContentType,
                            Extension = string.IsNullOrWhiteSpace(fileUploadField.FileName) ? "" : fileUploadField.FileName.Substring(fileUploadField.FileName.IndexOf('.') + 1),
                            Data = fileUploadField.FileBytes
                        };
                        new FileInfo(Server.MapPath(direction)).Delete();
                        
                    }

                }
                else
                {
                    Ultility.CreateMessageBox(title: RequestMessage.Notify, message: "Chỉ cho phép dung lượng import tối đa là 300KB");
                    return this.Direct();
                }
            }
            else
            {
                Ultility.CreateMessageBox(title: RequestMessage.Notify, message: RequestMessage.NotFileUpload);
            }
            return this.Direct(direction);
        }
        public ActionResult GetField(StoreRequestParameters parameters)
        {
            return this.Store(lstFieldItem.OrderBy(i => i.Postition));
        }

        public ActionResult GetListValue()
        {
            var result = new List<object>();
            foreach (var pair in dictFieldValue)
            {
                result.Add(new
                {
                    ID = pair.Key,
                    Name = pair.Value
                });
            }
            
            return this.Store(result);
        }
        public DirectResult HandleChangesFieldExport(int id, string name, string field, int position)
        {
            var dict = dictFieldValue;
            var obj = lstFieldItem.FirstOrDefault(i => i.ID == id);
            obj.Name = name;
            obj.Field = field;
            if (dict.ContainsKey(field))
                obj.Value = dict[field];
            if (obj.Postition > position)
            {
                for (int i = position; i < obj.Postition; i++)
                {
                    lstFieldItem[i - 1].Postition++;
                }
                obj.Postition = position;
                lstFieldItem = lstFieldItem.OrderBy(o => o.Postition).ToList();
            }
            else if (obj.Postition < position)
            {
                for (int i = obj.Postition.Value; i <= position; i++)
                {
                    lstFieldItem[i - 1].Postition--;
                }
                obj.Postition = position;
                lstFieldItem = lstFieldItem.OrderBy(o => o.Postition).ToList();
            }

            X.GetCmp<Store>("stField").Reload();
            return this.Direct();
        }
        public ActionResult DeleteField(int ID = 0)
        {
            try
            {
                if (ID != 0)
                {
                    TemplateExportFieldItem obj = lstFieldItem.FirstOrDefault(i => i.ID == ID);
                    if (obj != null)
                        lstFieldItem.Remove(obj);
                    int temp = 1;
                    foreach (var item in lstFieldItem)
                    {
                        item.Postition = temp++;
                    }
                    Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("stField").Reload();
            }
            return this.Direct();
        }
        public ActionResult UpdateTemplate(TemplateExportItem data)
        {
            try
            {
                TemplateExportFieldSV tfSV = new TemplateExportFieldSV();
                var fileUploadField = X.GetCmp<FileUploadField>("FileImportField");
                if (fileItem!=null)
                {
                    data.File = fileItem;
                    data.Type = 2;
                    int temp_ID = new TemplateExportSV().Insert(data, User.ID);
                    if (temp_ID > 0)
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
                Ultility.CreateNotification(message: RequestMessage.CreateError, error: true);
            }            
            return this.Direct(true);
        }
    }
}
