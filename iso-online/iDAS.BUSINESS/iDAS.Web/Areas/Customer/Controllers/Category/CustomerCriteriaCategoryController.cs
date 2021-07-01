using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Core;
using iDAS.Services;
using iDAS.Items;
using iDAS.Utilities;

namespace iDAS.Web.Areas.Customer.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Tiêu chí đánh giá thỏa mãn", IsActive = true, Icon = "TagBlue", IsShow = true, Position = 3, Parent = GroupCategoryController.Code)]
    public class CustomerCriteriaCategoryController : BaseController
    {
        private CustomerCriteriaCategorySV categorySV = new CustomerCriteriaCategorySV();
        private CustomerCriteriaSV criteriaSV = new CustomerCriteriaSV();
        #region Danh sách tiêu chí
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xem danh sách")]
        public ActionResult Index()
        {
            return View();
        }
        public StoreResult LoadCategory(string node)
        {
            var nodes = new NodeCollection();
            var lsDataNode = categorySV.GetTreeData(node);
            foreach (var dataNode in lsDataNode)
            {
                Node nodeItem = new Node();
                nodeItem.NodeID = dataNode.ID.ToString();
                nodeItem.Text = dataNode.Name.ToUpper();
                nodeItem.Icon = Icon.Folder;
                nodeItem.Leaf = dataNode.Leaf;
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "ParentID", Value = dataNode.ParentID.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "Name", Value = dataNode.Name.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "ID", Value = JSON.Serialize(dataNode.ID), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "IsActive", Value = JSON.Serialize(dataNode.IsActive), Mode = ParameterMode.Value });
                nodes.Add(nodeItem);
            }
            return this.Store(nodes);
        }
        public ActionResult LoadCategoryCriteriaActive()
        {
            var result = new List<CustomerCriteriaCategoryItem>();
            result = categorySV.GetActive();
           return this.Store(result);
        }
        public ActionResult LoadData(StoreRequestParameters parameters, int cateId = 0)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var lstData = criteriaSV.GetByCategory(filter, cateId);
            return this.Store(lstData, filter.PageTotal);
        }
        #endregion

        #region Bộ tiêu chí

        #region Thêm mới
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Thêm bộ tiêu chí")]
        [HttpGet]
        public ActionResult InsertCategory(int parentId)
        {
            try
            {
                var data = new CustomerCriteriaCategoryItem()
                {
                    ParentID = parentId,
                };
                return new Ext.Net.MVC.PartialViewResult { ViewName = "InsertCategory", Model = data };
            }
            catch
            {
                return this.Direct();
            }
        }
        [HttpPost]
        public ActionResult InsertCategory(CustomerCriteriaCategoryItem objNew)
        {
            try
            {
                if (categorySV.Insert(objNew, User.ID))
                {
                    X.GetCmp<FormPanel>("frCriteriaCategory").Reset();
                    X.GetCmp<Store>("stCriteriaCategory").Reload();
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
                else
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.ERROR,
                        Title = "Thông báo",
                        Message = "Tên bộ tiêu chí đánh giá đã tồn tại!"
                    });
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
            }
            return this.Direct();
        }
        #endregion

        #region Cập nhật
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Sửa bộ tiêu chí")]
        [HttpGet]
        public ActionResult UpdateCategory(int id)
        {
            try
            {
                return new Ext.Net.MVC.PartialViewResult { ViewName = "UpdateCategory", Model = categorySV.GetById(id) }; 
            }
            catch
            {
                return this.Direct();
            }
        }
        [HttpPost]
        public ActionResult UpdateCategory(CustomerCriteriaCategoryItem objEdit)
        {
            try
            {
                if (categorySV.Update(objEdit, User.ID))
                {
                    X.GetCmp<FormPanel>("frCriteriaCategory").Reset();
                    X.GetCmp<Store>("stCriteriaCategory").Reload();
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
                else
                {
                    Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
                }
                return this.Direct();
            }
            catch
            {
                return this.Direct("Error");
            }
        }
        #endregion

        #region Xem chi tiết 
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xem bộ tiêu chí")]
        public ActionResult DetailCategory(int id)
        {
            try
            {
                return new Ext.Net.MVC.PartialViewResult { ViewName = "DetailCategory", Model = categorySV.GetById(id)};
            }
            catch
            {
                return this.Direct();
            }
        }
        #endregion

        #region Xóa bộ tiêu chí
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xóa bộ tiêu chí")]
        public ActionResult DeleteCategory(int id)
        {
            try
            {
                if (categorySV.Delete(id))
                {
                    X.GetCmp<Store>("stCriteriaCategory").Reload();
                    Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                    return this.Direct();
                }
                else
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.ERROR,
                        Title = "Thông báo",
                        Message = "Bộ tiêu chí đánh giá đã được sử dụng không được xóa!"
                    });
                }
            }
            catch { }
            X.Msg.Show(new MessageBoxConfig
            {
                Buttons = MessageBox.Button.OK,
                Icon = MessageBox.Icon.ERROR,
                Title = "Thông báo",
                Message = "Bộ tiêu chí đánh giá đã được sử dụng không được xóa!"
            });
            return this.Direct("Error");
        }
        #endregion

        #endregion

        #region Tiêu chí đánh giá

        #region Thêm
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Thêm")]
        [HttpGet]
        public ActionResult Insert(int cateId, string cateName)
        {
            var data = new CustomerCriteriaItem()
            {
                CategoryID = cateId,
                CategoryName = cateName
            };
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Insert", Model = data };
        }
        [HttpPost]
        public ActionResult Insert(CustomerCriteriaItem objNew)
        {
            try
            {
                if (criteriaSV.Insert(objNew, User.ID))
                {
                    X.GetCmp<Store>("StoreTreeCriteria").Reload();
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
                else
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.ERROR,
                        Title = "Thông báo",
                        Message = "Tên tiêu chí đánh giá đã tồn tại!"
                    });
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.CreateError);
            }
            return this.Direct();
        }
        #endregion

        #region Sửa
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Sửa")]
        [HttpGet]
        public ActionResult Update(int id)
        {
            var data = criteriaSV.GetById(id);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };
        }
        [HttpPost]
        public ActionResult Update(CustomerCriteriaItem objEdit)
        {
            try
            {
                if (criteriaSV.Update(objEdit, User.ID))
                {
                    X.GetCmp<FormPanel>("frCriteria").Reset();
                    X.GetCmp<Store>("StoreTreeCriteria").Reload();
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
                else
                {
                    Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
                }
                return this.Direct();
            }
            catch
            {
                return this.Direct("Error");
            }
        }
        #endregion

        #region Xóa
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xóa")]
        public ActionResult Delete(int id)
        {
            try
            {
                criteriaSV.Delete(id);
                X.GetCmp<Store>("StoreTreeCriteria").Reload();
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                return this.Direct();
            }
            catch
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.ERROR,
                    Title = "Thông báo",
                    Message = "Tiêu chí đánh giá đã được sử dụng không được xóa!"
                });
                return this.Direct("Error");
            }
        }
        #endregion

        #region Xem
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xem")]
        public ActionResult Detail(int id)
        {
            try
            {
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = criteriaSV.GetById(id) };
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
        #endregion

        #endregion
    }
}
