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
    [BaseSystem(Name = "Nhóm khách hàng", IsActive = true, IsShow = true, Position = 3, Icon = "Group")]
    public class GroupCustomerController : BaseController
    {
        private CustomerCategorySV CustomerCategoryServices = new CustomerCategorySV();
        [BaseSystem(Name = "Xem danh sách")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            ViewData["Administrator"] = User.Administrator;
            CustomerCategoryServices.UpdateRoot(User.ID);
            return PartialView();
        }
        public ActionResult LoadData(string node)
        {
            NodeCollection nodes = new NodeCollection();
            int nodeId = node == "root" ? 0 : System.Convert.ToInt32(node);
            var lsDataNodes = CustomerCategoryServices.GetTreeAllData(nodeId);
            foreach (var dataNode in lsDataNodes)
            {
                Node nodeItem = new Node();
                nodeItem.NodeID = dataNode.ID.ToString();
                nodeItem.Text = dataNode.Name;
                nodeItem.Icon = dataNode.ParentID == 0 ? Icon.House : Icon.Door;
                if (dataNode.IsEmployee)
                {
                    nodeItem.Cls = "ForEmployee";
                }
                nodeItem.Leaf = dataNode.Leaf;
                //if (!dataNode.Leaf) nodeItem.Expanded = true;
                //else nodeItem.Expanded = false;
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "ParentID", Value = dataNode.ParentID.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "DepartmentName", Value = dataNode.Departments.Name.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "UpdateAt", Value = dataNode.UpdateAt.Value.ToString("dd/MM/yyyy"), Mode = ParameterMode.Value });
                nodes.Add(nodeItem);
            }
            return this.Content(nodes.ToJson());
        }

        #region Cập nhật nhóm khách hàng
        [BaseSystem(Name = "Thêm mới và sửa")]
        [SystemAuthorize(CheckAuthorize = true)]
        [HttpGet]
        public ActionResult Update(int parentId, int groupCustomerID = 0)
        {
            var data = new CustomerCategoryItem();
            if (groupCustomerID != 0)
            {
                data = CustomerCategoryServices.GetById(groupCustomerID);
                data.ActionForm = Form.Edit;
            }
            else
            {
                data.ParentID = parentId;
                data.ActionForm = Form.Add;
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };
        }
        [HttpPost]
        public ActionResult Update(CustomerCategoryItem data)
        {
            int id = 0;
            try
            {
                if (data.ID != 0)
                {
                    CustomerCategoryServices.Update(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                    id = data.ID;
                }
                else
                {
                    id = CustomerCategoryServices.Insert(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            return this.Direct(id);
        }
        #endregion

        #region Xóa nhóm khách hàng
        [BaseSystem(Name = "Xóa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Delete(int id = 0)
        {
            bool success = false;
            try
            {
                if (id != 0)
                {
                    if (CustomerCategoryServices.Delete(id))
                    {
                        Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                        success = true;
                        X.GetCmp<TreeStore>("StoreGroupCustomer").Reload();
                    }
                    else
                    {
                        Ultility.CreateMessageBox(title: RequestMessage.Notify, message: RequestMessage.DataUsing, icon: MessageBox.Icon.ERROR, button: MessageBox.Button.OK);
                    }

                }
            }
            catch
            {
                Ultility.CreateMessageBox(title: RequestMessage.Notify, message: RequestMessage.DataUsing, icon: MessageBox.Icon.ERROR, button: MessageBox.Button.OK);
            }
            return this.Direct(success);
        }
        #endregion

        #region Xem chi tiết nhóm khách hàng
        [BaseSystem(Name = "Xem chi tiết")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult DetailForm(int groupCustomerID = 0)
        {
            var data = new CustomerCategoryItem();
            if (groupCustomerID != 0)
            {
                data = CustomerCategoryServices.GetById(groupCustomerID);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }
        #endregion

        #region Dùng chung
        public ActionResult MultipleDetail(string ids)
        {
            var result = new List<CustomerCategoryDetailItem>();
            if (!string.IsNullOrWhiteSpace(ids))
            {
                var intIds = ids.Split(',').Select(i => System.Convert.ToInt32(i)).ToArray();
                result = CustomerCategoryServices.GetByIds(intIds);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "MultipleDetail", Model = result };
        }
        public ActionResult MultipleSelect(string ids = "")
        {
            ViewData["Selected"] = ids;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "MultipleSelect", ViewData = ViewData };
        }
        public ActionResult LoadCategorySelected(string node, string IdSelects)
        {
            try
            {
                NodeCollection nodes = new NodeCollection();
                int nodeId = node == "root" ? 0 : System.Convert.ToInt32(node);
                var lsDataNodes = CustomerCategoryServices.GetTreeData(nodeId, User.EmployeeID, User.Administrator);
                var IdSelectArray = IdSelects.Split(',');
                foreach (var dataNode in lsDataNodes)
                {
                    Node nodeItem = new Node();
                    nodeItem.NodeID = dataNode.ID.ToString();
                    nodeItem.Text = dataNode.Name;
                    nodeItem.Icon = dataNode.ParentID == 0 ? Icon.House : Icon.Door;
                    if (dataNode.IsEmployee)
                    {
                        nodeItem.Cls = "ForEmployee";
                    }
                    if (dataNode.IsParent)
                    {
                        nodeItem.Cls = "NotAllow";
                    }
                    nodeItem.Leaf = dataNode.Leaf;
                    if (!dataNode.Leaf) nodeItem.Expanded = true;
                    else nodeItem.Expanded = false;
                    bool IsSelected = IdSelectArray.Where(i => i == dataNode.ID.ToString()).FirstOrDefault() != null;
                    nodeItem.CustomAttributes.Add(new ConfigItem { Name = "ParentID", Value = dataNode.ParentID.ToString(), Mode = ParameterMode.Value });
                    nodeItem.CustomAttributes.Add(new ConfigItem { Name = "IsParent", Value = dataNode.IsParent.ToString(), Mode = ParameterMode.Value });
                    nodeItem.CustomAttributes.Add(new ConfigItem { Name = "IsSelect", Value = JSON.Serialize(IsSelected), Mode = ParameterMode.Value });
                    nodes.Add(nodeItem);
                }
                return this.Content(nodes.ToJson());
            }
            catch
            {
                return this.Content(string.Empty);
            }
        }
        public ActionResult List()
        {
            ViewData["Administrator"] = User.Administrator;
            return PartialView();
        }
        // Du lieu khach hang dung chung
        public ActionResult LoadTreeData(string node)
        {
            NodeCollection nodes = new NodeCollection();
            int nodeId = node == "root" ? 0 : System.Convert.ToInt32(node);
            var lsDataNodes = CustomerCategoryServices.GetTreeData(nodeId, User.EmployeeID, User.Administrator);
            foreach (var dataNode in lsDataNodes)
            {
                Node nodeItem = new Node();
                nodeItem.NodeID = dataNode.ID.ToString();
                nodeItem.Text = dataNode.Name;
                nodeItem.Icon = dataNode.ParentID == 0 ? Icon.House : Icon.Door;
                if (dataNode.IsEmployee)
                {
                    nodeItem.Cls = "ForEmployee";
                }
                if (dataNode.IsParent)
                {
                    nodeItem.Cls = "NotAllow";
                }
                nodeItem.Leaf = dataNode.Leaf;
                //if (!dataNode.Leaf) nodeItem.Expanded = true;
                //else nodeItem.Expanded = false;
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "ParentID", Value = dataNode.ParentID.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "IsParent", Value = dataNode.IsParent.ToString(), Mode = ParameterMode.Value });
                nodes.Add(nodeItem);
            }
            return this.Content(nodes.ToJson());
        }
        #endregion
    }
}
