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
using System.IO;
using iDAS.Utilities;
namespace iDAS.Web.Areas.Customer.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Khách hàng thực tế", IsActive = true, Icon = "UserGray", IsShow = true, Position = 7)]
    public class CustomerOfficialController : BaseController
    {
        private CustomerSV customerSV = new CustomerSV();
        [BaseSystem(Name = "Xem danh sách")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadData(StoreRequestParameters parameters, int groupCustomerID = 0)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var result = new List<CustomerItem>();
            if (groupCustomerID != 0)
            {
                result = customerSV.GetOfficicalCustomerByGroupID(filter, groupCustomerID, User.EmployeeID, User.Administrator);
            }
            return this.Store(result, filter.PageTotal);
        }
        #region Thêm
        [BaseSystem(Name = "Thêm")]
        [SystemAuthorize(CheckAuthorize = true)]
        [HttpGet]
        public ActionResult Insert(int id = 0, string groupCustomerID = "", string groupName = "", bool IsPotential = false, bool IsOfficial = false)
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
            if (id != 0)
            {
                data = customerSV.GetById(id);
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
        public ActionResult Update(int id = 0, string groupCustomerID = "", string groupName = "", bool IsPotential = false, bool IsOfficial = false)
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
            if (id != 0)
            {
                data = customerSV.GetById(id);
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

        #region Xem chi tiết khách hàng thực tế
        [BaseSystem(Name = "Xem chi tiết")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult DetailForm(int id = 0)
        {
            var data = new CustomerItem();
            if (id != 0)
            {
                data = customerSV.GetById(id);
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

        #region Chăm sóc
        [BaseSystem(Name = "Chăm sóc")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult CareResult(int id)
        {
            return this.RedirectToActionPermanent("CareForm", "CustomerCareResult", new { id = id });
        }
        #endregion

        #region Đánh giá thỏa mãn
        [BaseSystem(Name = "Đánh giá thỏa mãn")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult AuditResult(int id)
        {
            return this.RedirectToActionPermanent("AuditForm", "CustomerAuditResult", new { id = id });
        }
        #endregion

        #region Liên hệ
        [BaseSystem(Name = "Liên hệ")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Contact(int id)
        {
            return this.RedirectToActionPermanent("Index", "CustomerContactHistory", new { id = id, isPotential = false, isOfficial = true });
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

        #region Đơn hàng
        [BaseSystem(Name = "Đơn hàng")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Order(int id)
        {
            return this.RedirectToActionPermanent("Index", "CustomerOrder", new { id = id });
        }
        #endregion

        #region Hợp đồng
        [BaseSystem(Name = "Hợp đồng")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Contract(int id)
        {
            return this.RedirectToActionPermanent("List", "CustomerContract", new { id = id });
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
        public ActionResult HasPotential(int id = 0)
        {
            try
            {
                if (id != 0)
                {
                    customerSV.HasPotential(id);
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

    }
}
