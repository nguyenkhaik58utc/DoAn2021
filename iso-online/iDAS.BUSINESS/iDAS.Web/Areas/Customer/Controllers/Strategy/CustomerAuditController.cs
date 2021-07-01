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
namespace iDAS.Web.Areas.Customer.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Quản lý đánh giá", Icon = "MedalGold2", IsActive = true, IsShow = true, Position = 2, Parent = GroupCommonController.Code)]
    public class CustomerAuditController : BaseController
    {
        private CustomerAssessSV customerAuditService = new CustomerAssessSV();
        private CustomerAssessObjectSV customerAduitSV = new CustomerAssessObjectSV();
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
        public ActionResult LoadCustomerAudit(StoreRequestParameters parameters)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var result = customerAuditService.GetAll(filter);
            return this.Store(result, filter.PageTotal);
        }

        #region Cập nhật

        [BaseSystem(Name = "Thêm mới và sửa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult UpdateForm(int id = 0)
        {
            var data = new CustomerAssessItem();
            if (id != 0)
            {
                data = customerAuditService.GetById(id);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };
        }

        public ActionResult Update(CustomerAssessItem data)
        {
            bool Success = false;
            try
            {
                if (data.ID != 0)
                {
                    customerAuditService.Update(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                    Success = true;
                }
                else
                {
                    customerAuditService.Insert(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                    Success = true;
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreCustomerAudit").Reload();
            }
            return this.FormPanel(Success);
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
                    customerAuditService.Delete(id);
                    Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreCustomerAudit").Reload();
            }
            return this.Direct();
        }
        #endregion

        #region Xem chi tiết
        [BaseSystem(Name = "Xem chi tiết")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult DetailForm(int id = 0)
        {
            var data = new CustomerAssessItem();
            if (id != 0)
            {
                data = customerAuditService.GetById(id);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }
        #endregion

        #region Nhóm khách hàng đánh giá
        [BaseSystem(Name = "Thiết lập đối tượng đánh giá")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ObjectForm(int id = 0)
        {
            var data = new CustomerAssessItem();
            data.ID = id;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Object", Model = data };
        }
        public ActionResult LoadCustomerAuditObject(StoreRequestParameters parameters, int id)
        {
            int totalCount;
            var result = customerAduitSV.GetAllCustomerAssessObject(parameters.Page, parameters.Limit, out totalCount, id);
            return this.Store(result, totalCount);
        }
        public ActionResult UpdateCustomerAduitObject(string data)
        {
            var CustomerCareObjectItem = Ext.Net.JSON.Deserialize<CustomerAssessObjectItem>(data);
            try
            {
                if (CustomerCareObjectItem.ID == null && CustomerCareObjectItem.IsSelect == true)
                {
                    customerAduitSV.Insert(CustomerCareObjectItem, User.ID);

                }
                if (CustomerCareObjectItem.ID != null && CustomerCareObjectItem.IsSelect == false)
                {
                    customerAduitSV.Delete((int)CustomerCareObjectItem.ID);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            X.GetCmp<Store>("stCustomerAuditObject").Reload();
            return this.Direct();
        }
        #endregion

        #region Gửi email Đánh giá cho khách hàng
        [BaseSystem(Name = "Gửi Email đánh giá")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult SendMailAudit(int id)
        {
            try
            {
                var request = Request.Url.Authority;
                customerAuditService.SendMailAudit(id, 10, request);
                Ultility.CreateNotification(message: RequestMessage.SendSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.SendError, error: true);
            }
            return this.Direct();
        }
        #endregion

        #region Thống kê đánh giá khách hàng
        [BaseSystem(Name = "Xem thống kê kết quả đánh giá")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ResultForm(int id = 0)
        {
            var data = new CustomerAssessItem();
            if (id != 0)
            {
                data = customerAuditService.GetById(id);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Result", Model = data };
        }
        public ActionResult LoadResult(string node, int auditId = 0, int categoryId = 0)
        {
            NodeCollection nodes = new NodeCollection();
            int parentId = node == "root" ? 0 : System.Convert.ToInt32(node);
            var groups = customerAuditService.GetCriteriasByParentID(parentId, auditId, categoryId);
            foreach (var group in groups)
            {
                nodes.Add(createNote(group, categoryId));
            }
            return this.Content(nodes.ToJson());
        }
        private Node createNote(SumCustomerAuditItem group, int cateId)
        {
            Node node = new Node();
            node.NodeID = group.ID.ToString();
            node.Expanded = false;
            node.Text = group.Name.ToUpper();
            node.Leaf = true;
            if (group.IsCategory)
            {
                node.Icon = Icon.Folder;
            }
            else
            {
                node.Icon = Icon.TagBlue;
            }
            node.CustomAttributes.Add(new ConfigItem { Name = "ParentID", Value = group.ParentID.ToString(), Mode = ParameterMode.Value });
            node.CustomAttributes.Add(new ConfigItem { Name = "ID", Value = JSON.Serialize(group.ID), Mode = ParameterMode.Value });
            node.CustomAttributes.Add(new ConfigItem { Name = "Name", Value = group.Name.ToString(), Mode = ParameterMode.Value });
            node.CustomAttributes.Add(new ConfigItem { Name = "SumPoint", Value = group.IsCategory ? string.Empty : group.SumPoint.ToString(), Mode = ParameterMode.Value });
            node.CustomAttributes.Add(new ConfigItem { Name = "SumCustomerAudit", Value = group.IsCategory ? string.Empty : group.SumCustomerAudit.ToString(), Mode = ParameterMode.Value });
            node.CustomAttributes.Add(new ConfigItem { Name = "IsCategory", Value = JSON.Serialize(group.IsCategory), Mode = ParameterMode.Value });
            return node;
        }
        #endregion

       
    }
}
