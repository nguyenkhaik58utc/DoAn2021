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
using System.Xml.Linq;
using System.Xml;
using System.Text;
using System.Data;
using System.Data.OleDb;

namespace iDAS.Web.Areas.Customer.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Khách hàng đã tiếp cận", IsActive = true, Icon = "UserGray", IsShow = true, Position = 5)]
    public class CustomerNormalController : BaseController
    {
        private CustomerSV customerSV = new CustomerSV();
        [BaseSystem(Name = "Xem danh sách")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Lấy danh sách 
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public ActionResult LoadData(StoreRequestParameters parameters, int groupCustomerID = 0)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var result = new List<CustomerItem>();
            if (groupCustomerID != 0)
            {
                result = customerSV.GetNormalCustomerByGroupID(filter, groupCustomerID, User.EmployeeID, User.Administrator);
            }
            return this.Store(result, filter.PageTotal);
        }

        #region Thêm
        [BaseSystem(Name = "Thêm")]
        [SystemAuthorize(CheckAuthorize = true)]
        [HttpGet]
        public ActionResult Insert(int normalCustomerID = 0, string groupCustomerID = "", string groupName = "", bool IsPotential = false, bool IsOfficial = false)
        {
            var data = new CustomerItem();
            data.ActionForm = Form.Add;
            if (!string.IsNullOrWhiteSpace(groupCustomerID))
            {
                data.CustomerCategory = new CustomerCategorySelectItem()
                {
                    GroupIds = groupCustomerID,
                    Name = "Nhóm khách hàng( " + groupName + ")"
                };
                data.Status = IsPotential ? "IsPotential" : (IsOfficial ? "IsOfficial" : "IsNew");
            }
            if (normalCustomerID != 0)
            {
                data = customerSV.GetById(normalCustomerID);
                data.ActionForm = Form.Edit;
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "InsertCustomer", Model = data };
        }
        [HttpPost]
        public ActionResult Insert(CustomerItem data)
        {
            try
            {
                data.IsPotential = data.Status == CustomerStatus.Potential;
                data.IsOfficial = data.Status == CustomerStatus.Official;
                var fileUploadField = X.GetCmp<FileUploadField>("FileUploadFieldId");
                if (fileUploadField.HasFile)
                {
                    data.ImageFile = new FileItem()
                    {
                        ID = Guid.NewGuid(),
                        Name = fileUploadField.FileName,
                        Size = fileUploadField.FileBytes.Length,
                        Type = fileUploadField.PostedFile.ContentType,
                        Extension = string.IsNullOrWhiteSpace(fileUploadField.FileName) ?
                                    "" : fileUploadField.FileName.Substring(fileUploadField.FileName.IndexOf('.') + 1),
                        Data = fileUploadField.FileBytes
                    };
                }
                if (data.ID != 0)
                {
                    customerSV.Update(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
                else
                {
                    data.ID = customerSV.Insert(data, User.ID);
                    X.GetCmp<Hidden>("hdfCustomerID").SetValue(data.ID);
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                    return this.Direct(data);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            return this.Direct();
        }
        #endregion

        #region Sửa
        [BaseSystem(Name = "Sửa")]
        [SystemAuthorize(CheckAuthorize = true)]
        [HttpGet]
        public ActionResult Update(int normalCustomerID = 0, string groupCustomerID = "", string groupName = "", bool IsPotential = false, bool IsOfficial = false)
        {
            var data = new CustomerItem();
            data.ActionForm = Form.Add;
            if (!string.IsNullOrWhiteSpace(groupCustomerID))
            {
                data.CustomerCategory = new CustomerCategorySelectItem()
                {
                    GroupIds = groupCustomerID,
                    Name = "Nhóm khách hàng( " + groupName + ")"
                };
                data.Status = IsPotential ? "IsPotential" : (IsOfficial ? "IsOfficial" : "IsNew");
            }
            if (normalCustomerID != 0)
            {
                data = customerSV.GetById(normalCustomerID);
                data.ActionForm = Form.Edit;
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "UpdateCustomer", Model = data };
        }
        [HttpPost]
        public ActionResult Update(CustomerItem data)
        {
            try
            {
                data.IsPotential = data.Status == CustomerStatus.Potential;
                data.IsOfficial = data.Status == CustomerStatus.Official;
                var fileUploadField = X.GetCmp<FileUploadField>("FileUploadFieldId");
                if (fileUploadField.HasFile)
                {
                    data.ImageFile = new FileItem()
                    {
                        ID = Guid.NewGuid(),
                        Name = fileUploadField.FileName,
                        Size = fileUploadField.FileBytes.Length,
                        Type = fileUploadField.PostedFile.ContentType,
                        Extension = string.IsNullOrWhiteSpace(fileUploadField.FileName) ?
                                    "" : fileUploadField.FileName.Substring(fileUploadField.FileName.IndexOf('.') + 1),
                        Data = fileUploadField.FileBytes
                    };
                }
                if (data.ID != 0)
                {
                    customerSV.Update(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
                else
                {
                    data.ID = customerSV.Insert(data, User.ID);
                    X.GetCmp<Hidden>("hdfCustomerID").SetValue(data.ID);
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                    return this.Direct(data);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            return this.Direct();
        }
        #endregion

        #region Xóa
        [BaseSystem(Name = "Xóa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Delete(int id = 0)
        {
            try
            {
                if (id != 0)
                {
                    customerSV.Delete(id);
                    Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreCustomer").Reload();
            }
            return this.Direct();
        }
        #endregion

        #region Xem chi tiết
        [BaseSystem(Name = "Xem chi tiết")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult DetailForm(int normalCustomerID = 0)
        {
            var data = new CustomerItem();
            if (normalCustomerID != 0)
            {
                data = customerSV.GetById(normalCustomerID);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "DetailCustomer", Model = data };
        }
        #endregion

        #region Khảo sát
        [BaseSystem(Name = "Khảo sát")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Survey(int id)
        {
            return this.RedirectToActionPermanent("SurveyForm", "CustomerSurveyResult", new { id = id });
        }
        #endregion

        #region Liên hệ
        [BaseSystem(Name = "Liên hệ")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Contact(int id)
        {
            return this.RedirectToActionPermanent("Index", "CustomerContactHistory", new { id = id });
        }
        [SystemAuthorize(CheckAuthorize = true, ActionCode = "Contact")]
        public ActionResult Appoinment(int id, string name)
        {
            return this.RedirectToActionPermanent("Appoinment", "CustomerAppoinment", new { id = id, name = name });
        }
        #endregion

        #region Phản hồi
        [BaseSystem(Name = "Phản hồi")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Feedback(int id)
        {
            return this.RedirectToActionPermanent("Index", "CustomerFeedback", new { id = id });
        }
        #endregion

        #region Xem lý lịch khách hàng
        [BaseSystem(Name = "Xem lý lịch")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Profile(int id)
        {
            return this.RedirectToActionPermanent("ProfileCustomerForm", "CustomerProfile", new { id = id });
        }
        #endregion

        #region Chuyển sang tiềm năng
        [SystemAuthorize(CheckAuthorize = true, ActionCode = "Update")]
        public ActionResult PotentialTransfer(int id = 0)
        {
            try
            {
                if (id != 0)
                {
                    customerSV.PotentialTransfer(id);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreCustomer").Reload();
            }
            return this.Direct();
        }
        #endregion

        #region ImportData
        [BaseSystem(Name = "Import dữ liệu")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ImportData(string groupCustomerID = "", string groupName = "")
        {
            ViewData["GroupCustomerID"] = groupCustomerID;
            ViewData["GroupName"] = groupName;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Import", ViewData = ViewData };
        }
        public ActionResult DataCustomerImport(StoreRequestParameters parameters, string direction)
        {
            var table = customerSV.ReadFileExcel(Server.MapPath(direction));
            var result = new List<object>();
            for (var i = 0; i < table.Rows.Count; i++)
            {
                result.Add(new
                {
                    A = table.Rows[i][0],
                    B = table.Rows[i][1],
                    C = table.Rows[i][2],
                    D = table.Rows[i][3],
                    E = table.Rows[i][4],
                    F = table.Rows[i][5],
                    G = table.Rows[i][6]
                });
            }
            return this.Store(result);
        }
        public ActionResult SelectImportFile()
        {
            var fileUploadField = X.GetCmp<FileUploadField>("FileImportField");
            var direction = Common.FileImportCustomer + Guid.NewGuid().ToString() + ".xlsx";
            if (fileUploadField.HasFile)
            {
                if (fileUploadField.PostedFile.ContentLength < 300 * 1024 + 1)
                {
                    if (!System.IO.File.Exists(Server.MapPath(direction)))
                    {
                        fileUploadField.PostedFile.SaveAs(Server.MapPath(direction));
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
        [HttpGet]
        public ActionResult SettingImport(string groupCustomerID, string groupName, string direction)
        {
            ViewData["GroupCustomerID"] = groupCustomerID;
            ViewData["GroupName"] = groupName;
            ViewData["Direction"] = direction;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "SettingImport", ViewData = ViewData };
        }
        [HttpPost]
        public ActionResult SettingImport(CustomerImportItem data, int groupId, string direction)
        {
            try
            {
                if (!string.IsNullOrEmpty(direction))
                {
                    var table = customerSV.ReadFileExcel(Server.MapPath(direction));
                    var customerImport = new List<CustomerItem>();
                    for (var i = 0; i < table.Rows.Count; i++)
                    {
                        customerImport.Add(new CustomerItem
                        {
                            Name = table.Rows[i][data.Name].ToString(),
                            TaxCode = table.Rows[i][data.TaxCode].ToString(),
                            Scope = table.Rows[i][data.Scope].ToString(),
                            Phone = table.Rows[i][data.Phone].ToString(),
                            Email = table.Rows[i][data.Email].ToString(),
                            Address = table.Rows[i][data.Address].ToString(),
                            Note = table.Rows[i][data.Note].ToString()
                        });
                    }
                    customerSV.Import(customerImport, groupId, User.ID);
                    System.IO.File.Delete(Server.MapPath(direction));
                }
                Ultility.CreateMessageBox(title: RequestMessage.Notify, message: RequestMessage.ImportSucess);
            }
            catch
            {
                Ultility.CreateMessageBox(title: RequestMessage.Notify, message: RequestMessage.ImportSucess);
            }
            return this.Direct();
        }
        #endregion
        [ValidateInput(false)]
        public DirectResult ExportExcel(int currentPage, int pageSize,int groupCustomerID = 0)
        {
            ModelFilter filter = new ModelFilter() { PageIndex = currentPage, PageSize = pageSize,SortDirection="desc",SortName="CreateAt" };
            var lst = customerSV.GetNormalCustomerByGroupID(filter, groupCustomerID, User.EmployeeID, User.Administrator);
            Dictionary<string, string> dictNameValue = new Dictionary<string, string>() { { "ID", "ID" }, { "Name", "Tên khách hàng" }, { "Scope", "Lĩnh vực" }, { "Phone", "Số điện thoại" }, { "Email", "Email" }, { "Address", "Địa chỉ" } };
            string _title = "Khách hàng tiếp cận";
            ExportToExcel.CreateExcelDocument<CustomerItem>(lst, "Temp.xlsx", this.Response, dictNameValue, _title);
            return this.Direct();
        }
    }
}
