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
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.IO;
namespace iDAS.Web.Areas.Customer.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Kiểm soát hợp đồng", Icon = "PagePortraitShot", IsActive = true, IsShow = true, Position = 8)]
    public class CustomerManagementContractController : BaseController
    {
        private CustomerContractSV CustomerContractService = new CustomerContractSV();
        private TemplateExportSV tempExportSV = new TemplateExportSV();
        public static List<TemplateExportFieldItem> lstFieldItem = new List<TemplateExportFieldItem>();

        public static int ContractID = 0;
        [BaseSystem(Name = "Xem danh sách")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Lấy danh sách hợp đồng theo khách hàng
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public ActionResult LoadContract(StoreRequestParameters parameters)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var result = CustomerContractService.GetByEmployee(filter, User.EmployeeID, User.Administrator);
            return this.Store(result, filter.PageTotal);
        }

        #region Cập nhật hợp đồng
        [BaseSystem(Name = "Thêm mới và sửa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult UpdateForm(int ID = 0, int customerID = 0)
        {
            var data = new CustomerContractItem();
            data.ActionForm = Form.Add;
            data.CustomerID = customerID;
            if (ID != 0)
            {
                data = CustomerContractService.GetById(ID);
                data.ActionForm = Form.Edit;
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };
        }

        public ActionResult Update(CustomerContractItem data)
        {
            try
            {
                if (data.ID != 0)
                {
                    CustomerContractService.Update(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
                else
                {
                    CustomerContractService.Insert(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Window>("winUpdateCustomerContract").Close();
                X.GetCmp<Store>("StoreCustomerContract").Reload();
            }
            return this.Direct();
        }
        #endregion

        #region Xóa hợp đồng
        [BaseSystem(Name = "Xóa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Delete(int ID = 0)
        {
            try
            {
                if (ID != 0)
                {
                    CustomerContractService.Delete(ID);
                    Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreCustomerContract").Reload();
            }
            return this.Direct();
        }
        #endregion

        #region Xem chi tiết hợp đồng
        [BaseSystem(Name = "Xem chi tiết")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult DetailForm(int ID = 0)
        {
            var data = new CustomerContractItem();
            if (ID != 0)
            {
                ContractID = ID;
                data = CustomerContractService.GetById(ID);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }
        public ActionResult LoadOrder(StoreRequestParameters parameters, int customerId, int contractId)
        {
            int totalCount;
            var result = CustomerContractService.GetOrder(parameters.Page, parameters.Limit, out totalCount, customerId, contractId);
            return this.Store(result, totalCount);
        }
        #endregion

        #region Thiết lập đề xuất thanh toán
        public ActionResult AccountingPaymentForm(int id)
        {
            var data = new CustomerContractItem() { ID = id };
            return new Ext.Net.MVC.PartialViewResult { ViewName = "AccountingPayment", Model = data };
        }
        public ActionResult LoadAccountingPayment(StoreRequestParameters parameters, int id)
        {
            int totalCount;
            var result = CustomerContractService.GetAccountingPayment(parameters.Page, parameters.Limit, out totalCount, id);
            return this.Store(result, totalCount);
        }
        public ActionResult LoadAccountingPaymentForAccounting(StoreRequestParameters parameters, int id)
        {
            int totalCount;
            var result = CustomerContractService.GetAccountingPaymentForAccounting(parameters.Page, parameters.Limit, out totalCount, id);
            return this.Store(result, totalCount);
        }
        public ActionResult LoadAccountingPaymentForCustomer(StoreRequestParameters parameters, int id)
        {
            int totalCount;
            var result = CustomerContractService.GetAccountingPaymentForCustomer(parameters.Page, parameters.Limit, out totalCount, id);
            return this.Store(result, totalCount);
        }
        #region Cập nhật đề xuất thanh toán
        public ActionResult AccountingPaymentUpdateForm(int id, int customerContractId)
        {
            var data = new AccountingPaymentItem();
            if (id != 0)
            {
                data = CustomerContractService.GetAccountingPaymentById(id);
                data.ActionForm = Form.Edit;
            }
            else
            {
                data.ActionForm = Form.Add;
                data.CustomerContractID = customerContractId;
                data.TotalContract = CustomerContractService.GetTotalByID(customerContractId);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "UpdateAccounting", Model = data };
        }
        public ActionResult UpdateAccounting(AccountingPaymentItem data)
        {
            try
            {
                if (data.ID != 0)
                {
                    CustomerContractService.AccountingPaymentUpdate(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
                else
                {
                    CustomerContractService.AccountingPaymentInsert(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.SendError, error: true);
            }
            return this.Direct();
        }
        #endregion

        /// <summary>
        /// Xem chi tiết đề xuất thanh toán
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult AccountingPaymentDeatailForm(int ID)
        {
            var data = new AccountingPaymentItem();
            if (ID != 0)
            {
                data = CustomerContractService.GetAccountingPaymentById(ID);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "DetailAccounting", Model = data };
        }
        public ActionResult DeleteAccountingPayment(int ID = 0)
        {
            try
            {
                if (ID != 0)
                {
                    CustomerContractService.AccountingPaymentDelete(ID);
                    Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreAccountingPayment").Reload();
            }
            return this.Direct();
        }
        #endregion
        public ActionResult AccountingPaymentDetailForm(int ID)
        {
            var data = new CustomerContractItem() { ID = ID };
            return new Ext.Net.MVC.PartialViewResult { ViewName = "AccountingPaymentDetail", Model = data };
        }
        #region Gửi cho kế toán

        public ActionResult SendAccounting(CustomerContractItem data)
        {
            try
            {
                CustomerContractService.SendAccounting(data, User.ID);
                Ultility.CreateNotification(message: RequestMessage.SendSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.SendError, error: true);
            }
            return this.Direct();
        }
        #endregion

        #region Phê duyệt
        public ActionResult ApproveForm(int ID)
        {
            var data = new CustomerContractItem();
            if (ID != 0)
            {
                data = CustomerContractService.GetById(ID);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Approve", Model = data };
        }
        public ActionResult SignCompany(CustomerContractItem data)
        {
            try
            {
                CustomerContractService.SignCompany(data);
                Ultility.CreateNotification(message: RequestMessage.ApproveSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.ApproveError, error: true);
            }
            return this.Direct();
        }
        #endregion

        #region Tạm dừng
        public ActionResult PauseContract(int ID = 0, bool IsPause = true)
        {
            try
            {
                if (ID != 0)
                {
                    CustomerContractService.Pause(ID, IsPause, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreCustomerContract").Reload();
            }
            return this.Direct();
        }
        #endregion

        #region Hủy hợp đồng
        public ActionResult CancelContractForm(int ID)
        {
            var data = new CustomerContractItem();
            if (ID != 0)
            {
                data = CustomerContractService.GetById(ID);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "CancelContract", Model = data };
        }
        public ActionResult CancelContract(CustomerContractItem data)
        {
            try
            {
                CustomerContractService.CancelContract(data, User.ID);
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                X.GetCmp<Store>("StoreCustomerContract").Reload();
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            return this.Direct();
        }
        #endregion

        #region Hoàn thành
        public ActionResult FinishContractForm(int ID)
        {
            var data = new CustomerContractItem();
            if (ID != 0)
            {
                data = CustomerContractService.GetById(ID);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "FinishContract", Model = data };
        }
        public ActionResult FinishContract(CustomerContractItem data)
        {
            try
            {
                CustomerContractService.FinishContract(data, User.ID);
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreCustomerContract").Reload();
            }
            return this.Direct();
        }
        #endregion
        #region Xuất word
        public ActionResult ExportData(string groupCustomerID = "", string groupName = "")
        {
            return new Ext.Net.MVC.PartialViewResult { ViewName = "ExportWord" };
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
        public ActionResult ExportFile(int tempID)
        {
            var direction = Common.FileTempExport + "temp" + DateTime.UtcNow.Ticks.ToString() + ".docx";
            var path = Server.MapPath(direction);
            var data = ExportWord.ItemToDictionary<CustomerContractItem>(CustomerContractService.GetById(ContractID));
            var tempExp = tempExportSV.GetByID(tempID);
            var lstTempField = new TemplateExportFieldSV().GetAllByTemID(tempID).OrderBy(m => m.Postition.Value).ToList();
            var filetest = new FileSV().GetById(new Guid("b9bc4090-debf-4e83-9209-0192b6410fcb"));
            System.IO.File.WriteAllBytes(Server.MapPath(Common.FileTempExport + "thanhnv.doc"), filetest.Data);
            Stream stream = new MemoryStream(tempExp.File.Data);
            if (tempExp.Type.HasValue && tempExp.Type == 1)
            {
                EditTag(stream, data, lstTempField);
            }
            else
                createNewFile(path, data, lstTempField);

            ExportWord.dowloadFile(path, this.Response, "word");
            //return File(tempExp.File.Data, "application/force-download");
            return this.Direct();
        }
        private void createNewFile(string path, Dictionary<string, string> dictValue, List<TemplateExportFieldItem> lstTempField)
        {
            try
            {
                using (WordprocessingDocument wordDoc = WordprocessingDocument.Create(path, WordprocessingDocumentType.Document))
                {
                    MainDocumentPart mainPart = wordDoc.AddMainDocumentPart();
                    mainPart.Document = new DocumentFormat.OpenXml.Wordprocessing.Document();
                    Body body = mainPart.Document.AppendChild(new Body());
                    StyleDefinitionsPart stylePart = mainPart.StyleDefinitionsPart;
                    if (stylePart == null)
                        stylePart = ExportWord.AddStylePartToPackage(wordDoc);
                    ExportWord.AddNewStyle(stylePart, "Default", "Default");
                    foreach (TemplateExportFieldItem item in lstTempField)
                    {
                        if (!string.IsNullOrEmpty(item.Field) && dictValue.ContainsKey(item.Field))
                        {
                            Paragraph pTitle = ExportWord.CreateParagraph(item.Name + dictValue[item.Field], "Default");
                            body.Append(pTitle);
                        }
                        else
                        {
                            Paragraph pTitle = ExportWord.CreateParagraph(item.Name, "Default");
                            body.Append(pTitle);
                        }
                    }
                    mainPart.Document.Save();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void EditTag(Stream stream, Dictionary<string, string> dictValue, List<TemplateExportFieldItem> lstTempField)
        {
            try
            {
                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(stream, true))
                {
                    var maintPart = wordDoc.MainDocumentPart;
                    foreach (TemplateExportFieldItem item in lstTempField)
                    {
                        if (!string.IsNullOrEmpty(item.Field) && dictValue.ContainsKey(item.Field))
                        {
                            var res = from bm in maintPart.Document.Body.Descendants()
                                      where bm.InnerText != string.Empty && bm.InnerText.Contains(item.Name) && bm.HasChildren == false
                                      select bm;
                            foreach (var i in res)
                            {
                                i.InsertAfterSelf(new Text(i.InnerText.Replace(item.Name, dictValue[item.Field])));
                                i.Remove();
                            }
                        }else
                        {
                            var res = from bm in maintPart.Document.Body.Descendants()
                                      where bm.InnerText != string.Empty && bm.InnerText.Contains(item.Name) && bm.HasChildren == false
                                      select bm;
                            foreach (var i in res)
                            {
                                i.InsertAfterSelf(new Text(i.InnerText.Replace(item.Name , "")));
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
        public ActionResult GetListTemp(StoreRequestParameters parameters)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var result = tempExportSV.GetAll(filter.PageIndex, filter.PageSize, out filter.PageTotal);
            return this.Store(result);
        }
        public ActionResult ShowFrmAddTemp(string path = "")
        {
            var data = new TemplateExportItem();
            //lstFieldItem = new List<TemplateExportFieldItem>();
            //if (string.IsNullOrEmpty(path))
            return new Ext.Net.MVC.PartialViewResult { ViewName = "InsertTempl", Model = data };
            //else
            //{
            //    data.Type = 1;
            //    List<string> lst = ExportWord.readListField(Server.MapPath(path));
            //    lstFieldItem = new List<TemplateExportFieldItem>();
            //    foreach (string name in lst)
            //    {
            //        if (name != "")
            //        {
            //            int _id = lstFieldItem.Count + 1;
            //            lstFieldItem.Add(new TemplateExportFieldItem() { ID = _id, Name = name, Postition = lstFieldItem.Count + 1 });
            //        }
            //    }
            //    return new Ext.Net.MVC.PartialViewResult { ViewName = "InsertTempl", Model = data };
            //}
        }
        public ActionResult ShowFrmUpdateTemp(int ID)
        {
            var data = tempExportSV.GetByID(ID);
            if (data != null)
            {
                lstFieldItem = new TemplateExportFieldSV().GetAllByTemID(ID);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "InsertTempl", Model = data };
        }
        public ActionResult GetField(StoreRequestParameters parameters)
        {
            return this.Store(lstFieldItem.OrderBy(i => i.Postition));
        }
        public ActionResult AddField()
        {
            X.Msg.Show(new MessageBoxConfig
            {
                Title = "Tên trường",
                Message = "Bạn hãy nhâp tên trường cần tạo vào đây:",
                Width = 300,
                Buttons = MessageBox.Button.OKCANCEL,
                Multiline = true,
                AnimEl = this.GetCmp<Button>("btnAddField").ClientID,
                Fn = new JFunction { Fn = "AddFieldToList" }
            });

            return this.Direct();
        }
        public ActionResult AddFieldToList(string name)
        {
            if (name != "")
            {
                int _id = lstFieldItem.Select(i => i.ID).ToList().Max() + 1;
                lstFieldItem.Add(new TemplateExportFieldItem() { ID = _id, Name = name, Postition = lstFieldItem.Count + 1 });
                this.GetCmp<Store>("stField").Reload();
            }
            return this.Direct();
        }
        public ActionResult GetListValue(StoreRequestParameters parameters)
        {
            var data = ExportWord.ItemToDictionary<CustomerContractItem>(CustomerContractService.GetById(ContractID));
            var result = new List<object>();
            foreach (var i in data)
            {
                result.Add(new
                {
                    ID = i.Key,
                    Name = i.Value
                });
            }
            return this.Store(result);
        }
        public DirectResult HandleChangesFieldExport(int id, string name, string field, int position)
        {
            var dict = ExportWord.ItemToDictionary<CustomerContractItem>(CustomerContractService.GetById(ContractID));
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
        public ActionResult UpdateTemplate(TemplateExportItem data)
        {
            try
            {
                TemplateExportFieldSV tfSV = new TemplateExportFieldSV();
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
                    data.Type = 1;
                }
                if (data.ID != 0)
                {
                    bool isUpdate = tempExportSV.Update(data, User.ID);
                    if (isUpdate)
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
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Window>("vpfrTempExport").Close();
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
        #endregion
    }
}
