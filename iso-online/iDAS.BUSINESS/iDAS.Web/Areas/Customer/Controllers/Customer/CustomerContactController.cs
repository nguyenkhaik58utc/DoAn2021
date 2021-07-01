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
    //Thông tin người liên hệ của khách hàng
    public class CustomerContactController : BaseController
    {
        private CustomerContactSV customerContactSV = new CustomerContactSV();
        public ActionResult LoadContactInfo(StoreRequestParameters parameters, int customerId = 0)
        {
            int totalCount = 0;
            var result = new List<CustomerContactItem>();
            if (customerId != 0)
            {
                result = customerContactSV.GetByCustomer(parameters.Page, parameters.Limit, out totalCount, customerId);
            }
            return this.Store(result, totalCount);
        }
        public ActionResult UpdateContactForm(int id = 0, int customerId = 0)
        {
            var data = new CustomerContactItem();
            data.ActionForm = Form.Add;
            data.CustomerID = customerId;
            if (id != 0)
            {
                data = customerContactSV.GetById(id);
                data.ActionForm = Form.Edit;
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "UpdateContact", Model = data };
        }
        public ActionResult UpdateContact(CustomerContactItem data)
        {
            try
            {
                var fileUploadField = X.GetCmp<FileUploadField>("FileUploadRepresentId");
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
                    customerContactSV.Update(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
                else
                {
                    customerContactSV.Insert(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            return this.Direct();
        }
        public ActionResult DetailContactForm(int id = 0)
        {
            var data = new CustomerContactItem();
            if (id != 0)
            {
                data = customerContactSV.GetById(id);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "DetailContact", Model = data };
        }
        public ActionResult DeleteContact(int id = 0)
        {
            try
            {
                if (id != 0)
                {
                    customerContactSV.Delete(id);
                    X.GetCmp<Store>("StoreCustomerContact").Reload();
                    Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
            }
            return this.Direct();
        }
    }
}
